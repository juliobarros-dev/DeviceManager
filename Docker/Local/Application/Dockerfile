# Etapa de build
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /app

# Copia a solution e os projetos
COPY DeviceManager.sln ./
COPY DeviceManager.Application.WebApi ./DeviceManager.Application.WebApi
COPY DeviceManager.Domain.Models ./DeviceManager.Domain.Models
COPY DeviceManager.Domain.Services ./DeviceManager.Domain.Services
COPY DeviceManager.Infrastructure.Database ./DeviceManager.Infrastructure.Database

# Restaura e publica
WORKDIR /app/DeviceManager.Application.WebApi
RUN dotnet restore
RUN dotnet publish -c Release -o /out

# Etapa de runtime
FROM mcr.microsoft.com/dotnet/aspnet:9.0
WORKDIR /app
COPY --from=build /out .

EXPOSE 8080
ENTRYPOINT ["dotnet", "DeviceManager.Application.WebApi.dll"]
