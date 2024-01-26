FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
ENV ASPNETCORE_HTTP_PORTS=5094
EXPOSE 5094

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["HW3_StoreWebApi_Storage/HW3_StoreWebApi_Storage.csproj", "HW3_StoreWebApi_Storage/"]
RUN dotnet restore "HW3_StoreWebApi_Storage/HW3_StoreWebApi_Storage.csproj"
COPY . .
WORKDIR "/src/HW3_StoreWebApi_Storage"
RUN dotnet build "HW3_StoreWebApi_Storage.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "HW3_StoreWebApi_Storage.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "HW3_StoreWebApi_Storage.dll"]
