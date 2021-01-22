FROM mcr.microsoft.com/dotnet/core/aspnet:3.1-alpine AS runtime

ENV ASPNETCORE_ENVIRONMENT=Production
ENV DOTNET_CLI_TELEMETRY_OPTOUT=true

EXPOSE 80

WORKDIR /app

FROM mcr.microsoft.com/dotnet/core/sdk:3.1-alpine AS build

ENV ASPNETCORE_ENVIRONMENT=Production
ENV DOTNET_CLI_TELEMETRY_OPTOUT=true

WORKDIR /src

# Sources
COPY . .

# Restore
RUN dotnet restore ./Area.API/*.csproj

# Build
RUN dotnet publish ./Area.API/*.csproj -c Release -o /app

# ---- Runtime image ----
FROM runtime

# SOURCES
COPY --from=build /app .

CMD dotnet Area.API.dll
