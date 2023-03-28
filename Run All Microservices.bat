@ECHO OFF
ECHO Run All Microservices.
dotnet run --project ./src/ApiGateways/ApiGatewayForWeb/ApiGateway.ForWeb.csproj
PAUSE

@ECHO OFF
dotnet run --project ./src/Services/Baskets/BasketService/BasketService.csproj
PAUSE

@ECHO OFF
dotnet run --project ./src/Services/Discounts/DiscountService/DiscountService.csproj
PAUSE

@ECHO OFF
dotnet run --project ./src/Identity/IdentityService/IdentityService.csproj
PAUSE

@ECHO OFF
dotnet run --project ./src/Services/Orders/OrderService/OrderService.csproj
PAUSE

@ECHO OFF
dotnet run --project ./src/Services/Payments/Presentation/PaymentService.EndPoint/PaymentService.EndPoint.csproj
PAUSE

@ECHO OFF
dotnet run --project ./src/Services/Products/ProductService/ProductService.csproj
PAUSE