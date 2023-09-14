#See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["src/ApiGateways/ApiGatewayForWeb/ApiGateway.ForWeb.csproj", "src/ApiGateways/ApiGatewayForWeb/"]
RUN dotnet restore "src/ApiGateways/ApiGatewayForWeb/ApiGateway.ForWeb.csproj"
COPY . .
WORKDIR "/src/src/ApiGateways/ApiGatewayForWeb"
RUN dotnet build "ApiGateway.ForWeb.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "ApiGateway.ForWeb.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "ApiGateway.ForWeb.dll"]