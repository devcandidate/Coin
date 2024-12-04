FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /app

COPY Coin.Api/*.csproj ./Coin.Api/
COPY Coin.Application/*.csproj ./Coin.Application/
COPY Coin.Infrastructure/*.csproj ./Coin.Infrastructure/

RUN dotnet restore ./Coin.Api/Coin.Api.csproj

COPY . .

RUN dotnet publish ./Coin.Api/Coin.Api.csproj -c Debug -o /out

FROM mcr.microsoft.com/dotnet/aspnet:8.0
WORKDIR /app
COPY --from=build /out .

EXPOSE 8080

ENTRYPOINT ["dotnet", "Coin.Api.dll"]