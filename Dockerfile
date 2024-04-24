FROM node:18 AS frontend-build
WORKDIR /app

COPY frontend/package.json frontend/package-lock.json ./
RUN npm install

COPY frontend/ ./
RUN npm run build

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS backend-build
WORKDIR /src

COPY backend/ /src/backend/

WORKDIR /src/backend
RUN dotnet restore
RUN dotnet publish -c Release -o /app/publish/backend

FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS final
WORKDIR /app

COPY --from=backend-build /app/publish/backend ./backend
COPY --from=frontend-build /app/dist ./wwwroot


EXPOSE 80

ENTRYPOINT ["dotnet", "backend/api.dll"]
