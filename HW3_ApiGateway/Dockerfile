FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
ENV ASPNETCORE_HTTP_PORTS=5206
EXPOSE 5206

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["HW3_ApiGateway/HW3_ApiGateway.csproj", "HW3_ApiGateway/"]
RUN dotnet restore "HW3_ApiGateway/HW3_ApiGateway.csproj"
COPY . .
WORKDIR "/src/HW3_ApiGateway"
RUN dotnet build "HW3_ApiGateway.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "HW3_ApiGateway.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "HW3_ApiGateway.dll"]
