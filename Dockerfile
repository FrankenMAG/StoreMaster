FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /src

# Copiar archivos de proyecto
COPY ["StoreMaster.Web/StoreMaster.Web.csproj", "StoreMaster.Web/"]
COPY ["StoreMaster.Core/StoreMaster.Core.csproj", "StoreMaster.Core/"]
COPY ["StoreMaster.Infrastructure/StoreMaster.Infrastructure.csproj", "StoreMaster.Infrastructure/"]

# Restaurar dependencias
RUN dotnet restore "StoreMaster.Web/StoreMaster.Web.csproj"

# Copiar todo el código
COPY . .

# Compilar
WORKDIR "/src/StoreMaster.Web"
RUN dotnet build "StoreMaster.Web.csproj" -c Release -o /app/build

# Publicar
FROM build AS publish
RUN dotnet publish "StoreMaster.Web.csproj" -c Release -o /app/publish

# Imagen final
FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "StoreMaster.Web.dll"]