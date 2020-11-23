FROM mcr.microsoft.com/dotnet/core/sdk:3.1-alpine as build

ENV ASPNETCORE_ENVIRONMENT=Production
ENV DOTNET_CLI_TELEMETRY_OPTOUT=true

WORKDIR /src

# Sources
COPY . .

# Restore
RUN dotnet restore ./Dashboard.API/*.csproj

# Build
RUN dotnet publish ./Dashboard.API/*.csproj -c Release -o /app

# ---- Runtime image ----
FROM mcr.microsoft.com/dotnet/core/aspnet:3.1-alpine

WORKDIR /app

ENV ASPNETCORE_ENVIRONMENT=Production
ENV DOTNET_CLI_TELEMETRY_OPTOUT=true

# SOURCES
COPY --from=build /app .

EXPOSE 80

CMD ["dotnet", "Dashboard.API.dll"]
