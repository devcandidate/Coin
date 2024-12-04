using Coin.Application.Services.Interfaces;
using Coin.Application.Services;
using Coin.Infrastructure.ExternalServices.Interfaces;
using RestSharp;
using Coin.Infrastructure.ExternalServices;
using Coin.Infrastructure.Persistance;
using Microsoft.EntityFrameworkCore;
using FluentValidation;
using Coin.Application.Behaviors;
using MediatR;
using Microsoft.AspNetCore.Diagnostics;
using Coin.Application.Features.ExchangeRates.Queries.GetExchangeRates;
using Hangfire;
using Hangfire.SqlServer;
using Coin.Application.Jobs;
using Hangfire.Dashboard;

var builder = WebApplication.CreateBuilder(args);


builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddScoped<INbpApiClient, NbpApiClient>();
builder.Services.AddScoped<IExchangeRatesSyncService, ExchangeRatesSyncService>();
builder.Services.AddScoped<ICurrencyService, CurrencyService>();

builder.Services.AddMediatR(cfg =>
{
    cfg.RegisterServicesFromAssembly(typeof(GetExchangeRatesQueryHandler).Assembly);
});

builder.Services.AddScoped<RestClient>();
builder.Services.AddValidatorsFromAssemblyContaining<GetExchangeRatesQueryValidator>();
builder.Services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>));

builder.Services.AddDbContext<ApplicationDbContext>(options => options.UseSqlServer(builder.Configuration.GetConnectionString("DbConnection")));


builder.Services.AddHangfire(config =>
{
    config.SetDataCompatibilityLevel(CompatibilityLevel.Version_170)
          .UseSimpleAssemblyNameTypeSerializer()
          .UseRecommendedSerializerSettings()
          .UseSqlServerStorage(builder.Configuration.GetConnectionString("DbConnection"), new SqlServerStorageOptions
          {
              CommandBatchMaxTimeout = TimeSpan.FromMinutes(5),
              SlidingInvisibilityTimeout = TimeSpan.FromMinutes(5),
              QueuePollInterval = TimeSpan.Zero,
              UseRecommendedIsolationLevel = true,
              DisableGlobalLocks = true
          });
});



builder.Services.AddHangfireServer();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

using (var scope = app.Services.CreateScope())
{
    var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
    context.Database.Migrate();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.UseExceptionHandler(exceptionHandlerApp =>
{
    exceptionHandlerApp.Run(async context =>
    {
        context.Response.ContentType = "application/json";

        var exception = context.Features.Get<IExceptionHandlerFeature>()?.Error;

        var logger = context.RequestServices.GetRequiredService<ILogger<Program>>();

        if (exception != null)
        {
            logger.LogError(exception, "An unhandled exception occurred.");
        }

        var (statusCode, message, details) = exception switch
        {
            OperationCanceledException => (
                499,
                "Request was canceled by the client.",
                null
            ),
            ValidationException validationException => (
                StatusCodes.Status400BadRequest,
                "Validation error occurred.",
                validationException.Errors.Select(e => new { e.PropertyName, e.ErrorMessage })
            ),
            UnauthorizedAccessException => (
                StatusCodes.Status401Unauthorized,
                "Unauthorized access.",
                null
            ),
            _ => (
                StatusCodes.Status500InternalServerError,
                "An unexpected error occurred.",
                null
            )
        };

        // Log specific cases if needed
        if (statusCode == StatusCodes.Status500InternalServerError)
        {
            logger.LogError(exception, "A server error occurred: {Message}", message);
        }
        else if (statusCode == StatusCodes.Status400BadRequest && details != null)
        {
            logger.LogWarning("Validation error occurred: {@Details}", details);
        }

        context.Response.StatusCode = statusCode;

        var errorResponse = new
        {
            StatusCode = statusCode,
            Message = message,
            Details = details
        };

        await context.Response.WriteAsJsonAsync(errorResponse);
    });
});

app.UseHangfireDashboard("/hangfire", new DashboardOptions { });

RecurringJob.AddOrUpdate<ExchangeRatesSyncJob>(
    nameof(ExchangeRatesSyncJob),
    job => job.SyncExchangeRatesAsync(),
    Cron.Daily(12, 05)
);

app.Run();
