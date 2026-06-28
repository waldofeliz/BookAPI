FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /src

COPY BookAPI/Directory.Build.props BookAPI/
COPY BookAPI/BookAPI.sln BookAPI/
COPY BookAPI/Api/Api.csproj BookAPI/Api/
COPY BookAPI/Application/Application.csproj BookAPI/Application/
COPY BookAPI/Domain/Domain.csproj BookAPI/Domain/
COPY BookAPI/Infrastructure/Infrastructure.csproj BookAPI/Infrastructure/
COPY BookAPI/Shared/Shared.csproj BookAPI/Shared/

RUN dotnet restore BookAPI/Api/Api.csproj

COPY BookAPI/ BookAPI/
RUN dotnet publish BookAPI/Api/Api.csproj -c Release -o /app/publish --no-restore

FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS runtime
WORKDIR /app

RUN apt-get update && apt-get install -y --no-install-recommends curl \
    && rm -rf /var/lib/apt/lists/*

ENV ASPNETCORE_URLS=http://+:8080
ENV ASPNETCORE_ENVIRONMENT=Production

EXPOSE 8080

COPY --from=build /app/publish .

HEALTHCHECK --interval=30s --timeout=5s --start-period=20s --retries=3 \
  CMD curl -f http://localhost:8080/health || exit 1

ENTRYPOINT ["dotnet", "Api.dll"]
