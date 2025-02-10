# Use official .NET runtime as base image
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 5001

# Build stage
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src
COPY ["TaskService.API/TaskService.API.csproj", "TaskService.API/"]
COPY ["TaskService.Application/TaskService.Application.csproj", "TaskService.Application/"]
COPY ["TaskService.Domain/TaskService.Domain.csproj", "TaskService.Domain/"]
COPY ["TaskService.Infrastructure/TaskService.Infrastructure.csproj", "TaskService.Infrastructure/"]
RUN dotnet restore "TaskService.API/TaskService.API.csproj"

# Copy and build app
COPY . .
WORKDIR "/src/TaskService.API"
RUN dotnet build -c Release -o /app/build
RUN dotnet publish -c Release -o /app/publish

# Final runtime image
FROM base AS final
WORKDIR /app
COPY --from=build /app/publish .
CMD ["dotnet", "TaskService.API.dll"]