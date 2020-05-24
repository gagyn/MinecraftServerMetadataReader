FROM mcr.microsoft.com/dotnet/core/sdk:3.1 AS build
WORKDIR /app

COPY MinecraftServerStatus.API/MinecraftServerStatus.API.csproj MinecraftServerStatus.API/MinecraftServerStatus.API.csproj
COPY MinecraftServerStatus.Commons/MinecraftServerStatus.Commons.csproj MinecraftServerStatus.Commons/MinecraftServerStatus.Commons.csproj
COPY MinecraftServerStatus.Controller/MinecraftServerStatus.Controller.csproj MinecraftServerStatus.Controller/MinecraftServerStatus.Controller.csproj
COPY MinecraftServerStatus.Domain/MinecraftServerStatus.Domain.csproj MinecraftServerStatus.Domain/MinecraftServerStatus.Domain.csproj
COPY MinecraftServerStatus.Integrations/MinecraftServerStatus.Integrations.csproj MinecraftServerStatus.Integrations/MinecraftServerStatus.Integrations.csproj
COPY MinecraftServerStatus.IoC/MinecraftServerStatus.IoC.csproj MinecraftServerStatus.IoC/MinecraftServerStatus.IoC.csproj
COPY . ./

RUN dotnet build MinecraftServerStatus.API/MinecraftServerStatus.API.csproj

RUN dotnet publish MinecraftServerStatus.API/MinecraftServerStatus.API.csproj -c Release -o out
FROM mcr.microsoft.com/dotnet/core/aspnet:3.1-buster-slim-arm32v7
WORKDIR /app
COPY --from=build /app/out .
ENTRYPOINT ["dotnet", "MinecraftServerStatus.API.dll"]
