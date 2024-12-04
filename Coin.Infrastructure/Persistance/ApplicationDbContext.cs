using Coin.Infrastructure.Persistance.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Coin.Infrastructure.Persistance
{
    public class ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : DbContext(options)
    {
        public DbSet<ExchangeRate> ExchangeRates { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<ExchangeRate>(entity =>
            {
                entity.HasKey(e => new { e.Currency, e.Date });

                entity.Property(e => e.Currency)
                      .IsRequired()
                      .HasMaxLength(3);

                entity.Property(e => e.Date)
                      .IsRequired()
                      .HasColumnType("date");

                entity.Property(e => e.Bid)
                      .HasColumnType("decimal(18,6)");

                entity.Property(e => e.Ask)
                      .HasColumnType("decimal(18,6)");

                entity.Property(e => e.Mid)
                      .HasColumnType("decimal(18,6)");

                entity.Property(e => e.LastModified)             
                      .HasDefaultValueSql("GETDATE()")  
                      .ValueGeneratedOnAddOrUpdate();
            });
        }
    }
}
