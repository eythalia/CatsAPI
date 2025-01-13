# Use the official .NET SDK image to build the application
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS base
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

# This stage is used to build the service project
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
# Copy project files and restore dependencies
#COPY *.sln .
COPY ["CatImageApi.API/CatImageApi.API.csproj", "CatImageApi.API/"]
COPY ["CatImageApi.Application/CatImageApi.Application.csproj", "CatImageApi.Application/"]
COPY ["CatImageApi.Domain/CatImageApi.Domain.csproj", "CatImageApi.Domain/"]
COPY ["CatImageApi.Infrastructure/CatImageApi.Infrastructure.csproj", "CatImageApi.Infrastructure/"]

RUN dotnet restore "CatImageApi.API/CatImageApi.API.csproj"
COPY . .
WORKDIR /src/CatImageApi.API
RUN dotnet build "./CatImageApi.API.csproj" -c $BUILD_CONFIGURATION -o /app/build

# This stage is used to publish the service project to be copied to the final stage
FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./CatImageApi.API.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

# This stage is used in production or when running from VS in regular mode (Default when not using the Debug configuration)
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "CatImageApi.API.dll"]
