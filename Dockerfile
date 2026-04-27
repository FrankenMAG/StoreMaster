# syntax=docker/dockerfile:1
FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /src

COPY StoreMaster.sln .
COPY StoreMaster.Web/StoreMaster.Web.csproj StoreMaster.Web/
COPY StoreMaster.Core/StoreMaster.Core.csproj StoreMaster.Core/
COPY StoreMaster.Infrastructure/StoreMaster.Infrastructure.csproj StoreMaster.Infrastructure/

RUN dotnet restore StoreMaster.sln

COPY . .

RUN dotnet publish StoreMaster.Web/StoreMaster.Web.csproj -c Release -o /app/publish

FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS final
WORKDIR /app
COPY --from=build /app/publish .
ENV ASPNETCORE_URLS=http://+:$PORT
EXPOSE $PORT
ENTRYPOINT ["dotnet", "StoreMaster.Web.dll"]