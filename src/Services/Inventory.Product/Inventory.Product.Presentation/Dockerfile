FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER $APP_UID
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["Services/Inventory.Product/Inventory.Product.API/Inventory.Product.API.csproj", "Services/Inventory.Product/Inventory.Product.API/"]
COPY ["Services/Inventory.Product/Inventory.Product.Repositories/Inventory.Product.Repositories.csproj", "Services/Inventory.Product/Inventory.Product.Repositories/"]
COPY ["Services/Inventory.Product/Inventory.Product.Services/Inventory.Product.Services.csproj", "Services/Inventory.Product/Inventory.Product.Services/"]
COPY ["BuildingBlocks/Shared/Shared.csproj", "BuildingBlocks/Shared/"]
COPY ["BuildingBlocks/Infrastructure/Infrastructure.csproj", "BuildingBlocks/Infrastructure/"]
COPY ["BuildingBlocks/Contracts/Contracts.csproj", "BuildingBlocks/Contracts/"]
COPY ["BuildingBlocks/EventBus/EventBus.Messages/EventBus.Messages.csproj", "BuildingBlocks/EventBus/EventBus.Messages/"]
COPY ["BuildingBlocks/Common.Logging/Common.Logging.csproj", "BuildingBlocks/Common.Logging/"]
RUN dotnet restore "Services/Inventory.Product/Inventory.Product.API/Inventory.Product.API.csproj"
COPY . .
WORKDIR "/src/Services/Inventory.Product/Inventory.Product.API"
RUN dotnet build "Inventory.Product.API.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "Inventory.Product.API.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Inventory.Product.API.dll"]
