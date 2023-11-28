FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src

COPY ["GeoLocatorAPIHub.API.csproj", "./"]
RUN dotnet restore "GeoLocatorAPIHub.API.csproj"

COPY . .
RUN dotnet build "GeoLocatorAPIHub.API.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "GeoLocatorAPIHub.API.csproj" -c Release -o /app/publish

FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "GeoLocatorAPIHub.API.dll"]