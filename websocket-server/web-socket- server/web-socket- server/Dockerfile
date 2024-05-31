FROM mcr.microsoft.com/dotnet/runtime:8.0 AS base
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src
COPY ["web-socket- server/web-socket- server.csproj", "web-socket- server/"]
RUN dotnet restore "web-socket- server/web-socket- server.csproj"
COPY . .
WORKDIR "/src/web-socket- server"
RUN dotnet build "web-socket- server.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "web-socket- server.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "web-socket- server.dll"]
