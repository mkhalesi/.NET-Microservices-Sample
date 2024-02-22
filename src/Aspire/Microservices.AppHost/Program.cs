
var builder = DistributedApplication.CreateBuilder(args);

var cache = builder.AddRedisContainer("rediscache");
var rabbitMq = builder.AddRabbitMQContainer("rabbitmq");
var mongo = builder.AddContainer("mongo", "mongo");

builder.AddProject<Projects.OrderService>("orderservice")
    .WithReference(cache)
    .WithReference(rabbitMq)
    .WithEnvironment("DatabaseSettings__ConnectionString", "mongodb://localhost:27017")
    .WithEnvironment("DatabaseSettings__DatabaseName", "MicroserviceOrderDB")
    .WithEnvironment("RabbitMq__Uri", "amqp://guest:guest@rabbitmq:5672");

builder.Build().Run();
