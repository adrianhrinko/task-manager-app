# Base runtime image
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS base
WORKDIR /src

# Watch debug build
FROM base AS debug
ENV DOTNET_USE_POLLING_FILE_WATCHER=1
ENV ASPNETCORE_ENVIRONMENT=Development
ENTRYPOINT ["dotnet", "watch", "run", "--project", "Services/TaskService/TaskService.API/TaskService.API.csproj", "--urls", "http://*:8080"]

# Build stage
FROM base AS build
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
RUN dotnet build -c Release -o /app/build
RUN dotnet publish -c Release -o /app/publish

# Release final image
FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS release
WORKDIR /app
EXPOSE 5001
COPY --from=release-build /app/publish .
ENV ASPNETCORE_ENVIRONMENT=Production
CMD ["dotnet", "TaskService.API.dll"]