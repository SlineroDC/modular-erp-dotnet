# 1. IMAGEN BASE
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 8080

# 2. CONSTRUCCIÓN
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Copiar proyectos y restaurar
COPY ["src/ERP.Api/ERP.Api.csproj", "src/ERP.Api/"]
COPY ["src/ERP.Core/ERP.Core.csproj", "src/ERP.Core/"]
COPY ["src/ERP.Infrastructure/ERP.Infrastructure.csproj", "src/ERP.Infrastructure/"]
RUN dotnet restore "src/ERP.Api/ERP.Api.csproj"

# Copiar todo el código
COPY . .
WORKDIR "/src/src/ERP.Api"
RUN dotnet build "ERP.Api.csproj" -c Release -o /app/build

# 3. PUBLICAR
FROM build AS publish
RUN dotnet publish "ERP.Api.csproj" -c Release -o /app/publish /p:UseAppHost=false

# 4. FINAL
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "ERP.Api.dll"]