
var builder = DistributedApplication.CreateBuilder(args);

var cache = builder.AddRedisContainer("rediscache");
var rabbitMq = builder.AddRabbitMQContainer("rabbitmq");
//var sqlserver = builder.AddSqlServerContainer("").AddDatabase("");
var mongo = builder.AddContainer("mongo", "mongo");

builder.AddProject<Projects.OrderService>("orderservice")
    .WithReference(cache)
    .WithReference(rabbitMq);

//.WithEnvironment("DatabaseSettings__ConnectionString", "mongodb://orderdb")
//.WithEnvironment("DatabaseSettings__DatabaseName", "MicroserviceOrderDB")
//.WithEnvironment("MicroServiceAddress__Product__Uri", "http://microservices-product.api")
//.WithEnvironment("Identity__Uri", "http://${EXTERNAL_DNS_NAME_OR_IP}:7018")
//.WithEnvironment("RabbitMq__Uri", "amqp://guest:guest@rabbitmq:5672")
//.WithReference(rabbitMq);

builder.Build().Run();
