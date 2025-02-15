# Base runtime image
FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS base
WORKDIR /app
EXPOSE 5001

# Build stage
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /src
COPY ["UserService.API/UserService.API.csproj", "UserService.API/"]
COPY ["UserService.Application/UserService.Application.csproj", "UserService.Application/"]
COPY ["UserService.Domain/UserService.Domain.csproj", "UserService.Domain/"]
COPY ["UserService.Infrastructure/UserService.Infrastructure.csproj", "UserService.Infrastructure/"]
RUN dotnet restore "UserService.API/UserService.API.csproj"
COPY . .
WORKDIR "/src/UserService.API"

# Debug build
FROM build AS debug-build
RUN dotnet build -c Debug -o /app/build
RUN dotnet publish -c Debug -o /app/publish

# Debug final image
FROM base AS debug
WORKDIR /app
EXPOSE 5901
RUN apt-get update \
    && apt-get install -y --no-install-recommends unzip procps \
    && rm -rf /var/lib/apt/lists/*
ENV ASPNETCORE_ENVIRONMENT=Development
COPY --from=debug-build /app/publish .
ENTRYPOINT ["dotnet", "UserService.API.dll"]

# Release build
FROM build AS release-build
RUN dotnet build -c Release -o /app/build
RUN dotnet publish -c Release -o /app/publish

# Release final image
FROM base AS release
WORKDIR /app
COPY --from=release-build /app/publish .
ENV ASPNETCORE_ENVIRONMENT=Production
CMD ["dotnet", "UserService.API.dll"]