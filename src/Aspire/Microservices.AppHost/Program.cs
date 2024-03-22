var builder = DistributedApplication.CreateBuilder(args);

var cache = builder.AddRedisContainer("rediscache");
var rabbitMq = builder.AddRabbitMQContainer("rabbitmq");
var mongo = builder.AddContainer("mongo", "mongo");

var sqlServerContainer = builder.AddSqlServerContainer("sqldata", null, 1434);
var basketConnectionString =
    "Server=localhost,1434;Database=MicroserviceBasketDB;User Id=sa;password=123321Aa@;Encrypt=False;TrustServerCertificate=true;";
//var sqlServerConnection = builder.AddSqlServerConnection("sqldata", basketConnectionString);

builder.AddProject<Projects.OrderService>("orderservice")
    .WithReference(cache)
    .WithReference(rabbitMq)
    .WithEnvironment("DatabaseSettings__ConnectionString", "mongodb://localhost:27017")
    .WithEnvironment("DatabaseSettings__DatabaseName", "MicroserviceOrderDB")
    .WithEnvironment("RabbitMq__Uri", "amqp://guest:guest@rabbitmq:5672");

builder.AddProject<Projects.BasketService>("basketservice")
    .WithReference(cache)
    .WithReference(rabbitMq)
    .WithReference(sqlServerContainer)
    .WithEnvironment("BasketConnection", basketConnectionString)
    .WithEnvironment("CacheSettings__ConnectionString", "rediscache")
    .WithEnvironment("RabbitMq__Uri", "amqp://guest:guest@rabbitmq:5672");


builder.Build().Run();
