FROM mcr.microsoft.com/dotnet/runtime:8.0 AS base
USER $APP_UID
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["UsageCollectorWorkerService/UsageCollectorWorkerService.csproj", "UsageCollectorWorkerService/"]
RUN dotnet restore "UsageCollectorWorkerService/UsageCollectorWorkerService.csproj"
COPY . .
WORKDIR "/src/UsageCollectorWorkerService"
RUN dotnet build "UsageCollectorWorkerService.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "UsageCollectorWorkerService.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "UsageCollectorWorkerService.dll"]
