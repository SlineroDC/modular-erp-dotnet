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

# Copiar proyectos y restaurar (Capa de caché de .NET)
COPY ["src/ERP.Admin/ERP.Admin.csproj", "src/ERP.Admin/"]
COPY ["src/ERP.Core/ERP.Core.csproj", "src/ERP.Core/"]
COPY ["src/ERP.Infrastructure/ERP.Infrastructure.csproj", "src/ERP.Infrastructure/"]
RUN dotnet restore "src/ERP.Admin/ERP.Admin.csproj"
RUN dotnet restore "src/ERP.Infrastructure/ERP.Infrastructure.csproj"

COPY . .

RUN npm install
RUN npm run build

# Ahora sí entramos a la carpeta del proyecto para compilar .NET
WORKDIR "/src/src/ERP.Admin"
RUN dotnet restore "ERP.Admin.csproj"
RUN dotnet build "ERP.Admin.csproj" -c Release -o /app/build

# 3. PUBLICAR
FROM build AS publish
RUN dotnet publish "ERP.Admin.csproj" -c Release -o /app/publish /p:UseAppHost=false

# 4. IMAGEN FINAL
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "ERP.Admin.dll"]