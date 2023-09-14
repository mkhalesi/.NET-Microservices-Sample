#See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:5.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:5.0 AS build
WORKDIR /src
COPY ["src/Services/Payments/Presentation/PaymentService.EndPoint/PaymentService.EndPoint.csproj", "src/Services/Payments/Presentation/PaymentService.EndPoint/"]
COPY ["src/Services/Payments/Core/PaymentService.Application/PaymentService.Application.csproj", "src/Services/Payments/Core/PaymentService.Application/"]
COPY ["src/Services/Payments/Core/PaymentService.Domain/PaymentService.Domain.csproj", "src/Services/Payments/Core/PaymentService.Domain/"]
COPY ["src/Services/Payments/Infrastructure/PaymentService.Infrastructure/PaymentService.Infrastructure.csproj", "src/Services/Payments/Infrastructure/PaymentService.Infrastructure/"]
COPY ["src/Services/Payments/Infrastructure/PaymentService.Persistence/PaymentService.Persistence.csproj", "src/Services/Payments/Infrastructure/PaymentService.Persistence/"]
RUN dotnet restore "src/Services/Payments/Presentation/PaymentService.EndPoint/PaymentService.EndPoint.csproj"
COPY . .
WORKDIR "/src/src/Services/Payments/Presentation/PaymentService.EndPoint"
RUN dotnet build "PaymentService.EndPoint.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "PaymentService.EndPoint.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "PaymentService.EndPoint.dll"]