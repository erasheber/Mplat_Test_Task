FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER $APP_UID
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["backend/Dotnet_Test_Task.Api/Dotnet_Test_Task.Api.csproj", "backend/Dotnet_Test_Task.Api/"]
COPY ["backend/Dotnet_Test_Task.Application/Dotnet_Test_Task.Application.csproj", "backend/Dotnet_Test_Task.Application/"]
COPY ["backend/Dotnet_Test_Task.Domain/Dotnet_Test_Task.Domain.csproj", "backend/Dotnet_Test_Task.Domain/"]
COPY ["backend/Dotnet_Test_Task.Infrastructure/Dotnet_Test_Task.Infrastructure.csproj", "backend/Dotnet_Test_Task.Infrastructure/"]
RUN dotnet restore "backend/Dotnet_Test_Task.Api/Dotnet_Test_Task.Api.csproj"
COPY . .
WORKDIR "/src/backend/Dotnet_Test_Task.Api"
RUN dotnet build "Dotnet_Test_Task.Api.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "Dotnet_Test_Task.Api.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Dotnet_Test_Task.Api.dll"]
