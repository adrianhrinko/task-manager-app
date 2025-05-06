# Base runtime image
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS base
WORKDIR /src

# Watch debug build
FROM base AS debug
ENV DOTNET_USE_POLLING_FILE_WATCHER=1
ENV ASPNETCORE_ENVIRONMENT=Development
ENTRYPOINT ["dotnet", "watch", "run", "--project", "Services/UserService/UserService.API/UserService.API.csproj", "--urls", "http://*:8080"]
# Build stage
FROM base AS build
COPY ["Services/UserService/UserService.API/UserService.API.csproj", "Services/UserService/UserService.API/"]
COPY ["Services/UserService/UserService.Application/UserService.Application.csproj", "Services/UserService/UserService.Application/"]
COPY ["Services/UserService/UserService.Domain/UserService.Domain.csproj", "Services/UserService/UserService.Domain/"]
COPY ["Services/UserService/UserService.Infrastructure/UserService.Infrastructure.csproj", "Services/UserService/UserService.Infrastructure/"]
COPY ["Shared/Shared.API/Shared.API.csproj", "Shared/Shared.API/"]
COPY ["Shared/Shared.Domain/Shared.Domain.csproj", "Shared/Shared.Domain/"]
COPY ["Shared/Shared.Infrastructure/Shared.Infrastructure.csproj", "Shared/Shared.Infrastructure/"]
RUN dotnet restore "Services/UserService/UserService.API/UserService.API.csproj"
COPY . .
WORKDIR "/src/Services/UserService/UserService.API"
RUN dotnet build -c Release -o /app/build
RUN dotnet publish -c Release -o /app/publish

# Release final image
FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS release
WORKDIR /app
EXPOSE 5001
COPY --from=release-build /app/publish .
ENV ASPNETCORE_ENVIRONMENT=Production
CMD ["dotnet", "UserService.API.dll"]