# 1. IMAGEN BASE
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

# 2. IMAGEN DE CONSTRUCCIÓN
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build

# Instalar Node.js
RUN apt-get update && \
    apt-get install -y curl && \
    curl -fsSL https://deb.nodesource.com/setup_20.x | bash - && \
    apt-get install -y nodejs

WORKDIR /src

# Copiar proyectos y restaurar
COPY ["src/ERP.Admin/ERP.Admin.csproj", "src/ERP.Admin/"]
COPY ["src/ERP.Core/ERP.Core.csproj", "src/ERP.Core/"]
COPY ["src/ERP.Infrastructure/ERP.Infrastructure.csproj", "src/ERP.Infrastructure/"]

RUN dotnet restore "src/ERP.Infrastructure/ERP.Infrastructure.csproj"
RUN dotnet restore "src/ERP.Admin/ERP.Admin.csproj"

# Copiar todo el código
COPY . .

# --- SOLUCIÓN NUCLEAR: Tailwind Standalone ---
# 1. Descargar el ejecutable oficial de Tailwind para Linux (Versión 3.4.1 estable)
RUN curl -sLO https://github.com/tailwindlabs/tailwindcss/releases/download/v3.4.1/tailwindcss-linux-x64

# 2. Darle permisos de ejecución
RUN chmod +x tailwindcss-linux-x64

# 3. Ejecutarlo directamente (Sin npm, sin npx, sin node_modules)
RUN ./tailwindcss-linux-x64 -i ./src/ERP.Admin/wwwroot/css/input.css -o ./src/ERP.Admin/wwwroot/css/site.css
# ---------------------------------------------
# -------------------------------------

# Compilar .NET
WORKDIR "/src/src/ERP.Admin"
RUN dotnet build "ERP.Admin.csproj" -c Release -o /app/build

# 3. PUBLICAR
FROM build AS publish
RUN dotnet publish "ERP.Admin.csproj" -c Release -o /app/publish /p:UseAppHost=false

# 4. IMAGEN FINAL
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "ERP.Admin.dll"]