FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
ARG APP_UID
USER $APP_UID
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

EXPOSE 4022

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["src/API/API.csproj", "src/API/"]
RUN dotnet restore "src/API/API.csproj"
COPY . .
WORKDIR "/src/src/API"
RUN dotnet build "API.csproj" -c $BUILD_CONFIGURATION -o /app/build /p:DebugType=full

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "API.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:DebugType=full /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "API.dll"]