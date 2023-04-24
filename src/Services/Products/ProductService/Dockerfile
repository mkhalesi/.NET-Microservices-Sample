#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:5.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:5.0 AS build
WORKDIR /src
COPY ["src/Services/Products/ProductService/ProductService.csproj", "src/Services/Products/ProductService/"]
RUN dotnet restore "src/Services/Products/ProductService/ProductService.csproj"
COPY . .
WORKDIR "/src/src/Services/Products/ProductService"
RUN dotnet build "ProductService.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "ProductService.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "ProductService.dll"]



#
##FROM mcr.microsoft.com/dotnet/aspnet:5.0 AS base
##WORKDIR /app
##EXPOSE 5000
##
##ENV ASPNETCORE_URLS=http://+:5000
##
##FROM mcr.microsoft.com/dotnet/sdk:5.0 AS build
##WORKDIR /src
##COPY ["ProductService/ProductService.csproj", "ProductService/"]
##RUN dotnet restore "ProductService\ProductService.csproj"
##COPY . .
##WORKDIR "/src/ProductService"
##RUN dotnet build "ProductService.csproj" -c Release -o /app/build
##
##FROM build AS publish
##RUN dotnet publish "ProductService.csproj" -c Release -o /app/publish /p:UseAppHost=false
##
##FROM base AS final
##WORKDIR /app
##COPY --from=publish /app/publish .
##ENTRYPOINT ["dotnet", "ProductService.dll"]
##
#
## syntax=docker/dockerfile:1
#FROM mcr.microsoft.com/dotnet/sdk:5.0 AS build-env
#WORKDIR /app
    #
## Copy csproj and restore as distinct layers
#COPY *.csproj ./
#RUN dotnet restore
    #
## Copy everything else and build
#COPY . ./
#RUN dotnet publish -c Release -o out
    #
## Build runtime image
#FROM mcr.microsoft.com/dotnet/aspnet:5.0
#WORKDIR /app
#EXPOSE 80
#COPY --from=build-env /app/out .
#ENTRYPOINT ["dotnet", "ProductService.dll"]
#
## docker run -e 'ACCEPT_EULA=Y' -e 'SA_PASSWORD=123321' -e 'MSSSQL_PID=Express' -p 1433:1433 -d mcr.microsoft.com/mssql/server:2017-latest-ubuntu
## docker run -e 'ACCEPT_EULA=Y' -e 'MSSSQL_PID=Express' -d mcr.microsoft.com/mssql/server:2017-latest
## complete docker-compose for sql instance and run
## docker run -e ‘ACCEPT_EULA=Y’ -e ‘SA_PASSWORD=Str0ngPa$$w0rd’ -p 1433:1433 -d mcr.microsoft.com/mssql/server
## docker run -e `ACCEPT_EULA=Y' -e `SA_PASSWORD=Str0ngPa$$w0rd' -p 1401:1433 -d mcr.microsoft.com/mssql/server:2017-latest
#