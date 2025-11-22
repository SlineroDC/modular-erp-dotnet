FROM mcr.microsoft.com/dotnet/runtime:8.0 AS base
USER $APP_UID
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["tests/ERP.Admin.Tests/ERP.Admin.Tests.csproj", "tests/ERP.Admin.Tests/"]
RUN dotnet restore "tests/ERP.Admin.Tests/ERP.Admin.Tests.csproj"
COPY . .
WORKDIR "/src/tests/ERP.Admin.Tests"
RUN dotnet build "./ERP.Admin.Tests.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./ERP.Admin.Tests.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "ERP.Admin.Tests.dll"]
