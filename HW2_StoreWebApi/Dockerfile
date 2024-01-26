FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
ENV ASPNETCORE_HTTP_PORTS=5204
EXPOSE 5204

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["HW2_StoreWebApi/HW2_StoreWebApi.csproj", "HW2_StoreWebApi/"]
RUN dotnet restore "HW2_StoreWebApi/HW2_StoreWebApi.csproj"
COPY . .
WORKDIR "/src/HW2_StoreWebApi"
RUN dotnet build "HW2_StoreWebApi.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "HW2_StoreWebApi.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "HW2_StoreWebApi.dll"]
