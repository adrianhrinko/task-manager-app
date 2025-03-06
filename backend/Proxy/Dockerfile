# Use official .NET SDK image to build the project
FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /src
COPY ["Proxy/TaskManager.Proxy/TaskManager.Proxy.csproj", "Proxy/TaskManager.Proxy/"]
RUN dotnet restore "Proxy/TaskManager.Proxy/TaskManager.Proxy.csproj"
COPY . .
WORKDIR "/src/Proxy/TaskManager.Proxy"
RUN dotnet publish -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=build /app/publish .
ENTRYPOINT ["dotnet", "TaskManager.Proxy.dll"]