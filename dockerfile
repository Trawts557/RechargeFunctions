FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /src

COPY . .
RUN dotnet restore ./RechargeFunctions/RechargeFunctions.Api.csproj
RUN dotnet publish ./RechargeFunctions/RechargeFunctions.Api.csproj -c Release -o /app/publish

FROM mcr.microsoft.com/dotnet/aspnet:9.0
WORKDIR /app
COPY --from=build /app/publish .

ENTRYPOINT ["dotnet", "RechargeFunctions.Api.dll"]
