FROM mcr.microsoft.com/dotnet/runtime:8.0 AS base
USER $APP_UID
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["tests/ERP.Api.Tests/ERP.Api.Tests.csproj", "tests/ERP.Api.Tests/"]
RUN dotnet restore "tests/ERP.Api.Tests/ERP.Api.Tests.csproj"
COPY . .
WORKDIR "/src/tests/ERP.Api.Tests"
RUN dotnet build "./ERP.Api.Tests.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./ERP.Api.Tests.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "ERP.Api.Tests.dll"]
