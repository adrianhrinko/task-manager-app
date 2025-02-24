# Base runtime image
FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS base
WORKDIR /app
EXPOSE 5001

# Build stage
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /src
COPY ["Services/TaskService/TaskService.API/TaskService.API.csproj", "Services/TaskService/TaskService.API/"]
COPY ["Services/TaskService/TaskService.Application/TaskService.Application.csproj", "Services/TaskService/TaskService.Application/"]
COPY ["Services/TaskService/TaskService.Domain/TaskService.Domain.csproj", "Services/TaskService/TaskService.Domain/"]
COPY ["Services/TaskService/TaskService.Infrastructure/TaskService.Infrastructure.csproj", "Services/TaskService/TaskService.Infrastructure/"]

COPY ["Shared/Shared.API/Shared.API.csproj", "Shared/Shared.API/"]
COPY ["Shared/Shared.Domain/Shared.Domain.csproj", "Shared/Shared.Domain/"]
COPY ["Shared/Shared.Infrastructure/Shared.Infrastructure.csproj", "Shared/Shared.Infrastructure/"]

RUN dotnet restore "Services/TaskService/TaskService.API/TaskService.API.csproj"
COPY . .
WORKDIR "/src/Services/TaskService/TaskService.API"

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
ENTRYPOINT ["dotnet", "TaskService.API.dll"]

# Release build
FROM build AS release-build
RUN dotnet build -c Release -o /app/build
RUN dotnet publish -c Release -o /app/publish

# Release final image
FROM base AS release
WORKDIR /app
COPY --from=release-build /app/publish .
ENV ASPNETCORE_ENVIRONMENT=Production
CMD ["dotnet", "TaskService.API.dll"]