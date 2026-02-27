
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

COPY WebApi.csproj ./WebApi/
RUN dotnet restore ./WebApi/WebApi.csproj

COPY . ./WebApi/
WORKDIR /src/WebApi
RUN dotnet publish -c Release -o /app/publish --no-restore

FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS runtime
WORKDIR /app

COPY --from=build /app/publish .

EXPOSE 80

ENTRYPOINT ["dotnet", "WebApi.dll"]
