FROM mcr.microsoft.com/dotnet/core/sdk:3.1 AS build
WORKDIR /app
COPY HypixelCounter/HypixelCounter.csproj HypixelCounter/HypixelCounter.csproj
COPY HypixelCounterServer/HypixelCounterServer.csproj HypixelCounterServer/HypixelCounterServer.csproj
COPY . ./

RUN dotnet build HypixelCounterServer/HypixelCounterServer.csproj

RUN dotnet publish HypixelCounterServer/HypixelCounterServer.csproj -c Release -o out
FROM mcr.microsoft.com/dotnet/core/runtime:3.1-buster-slim-arm32v7
WORKDIR /app
COPY --from=build /app/out .
ENTRYPOINT ["dotnet", "HypixelCounterServer.dll"]
