using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using OrderService.Infrastructure.Context;
using OrderService.MessageBus.Base;
using OrderService.MessageBus.RecievedMessage;
using OrderService.MessageBus.RecievedMessage.ProductMessages;
using OrderService.MessageBus.SendMessages;
using OrderService.Model.Services.OrderService;
using OrderService.Model.Services.ProductService;
using OrderService.Model.Services.RegisterOrderService;
using RestSharp;

var builder = WebApplication.CreateBuilder(args);

builder.AddServiceDefaults();

IConfiguration configuration = builder.Configuration;

builder.Services.AddControllers();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "OrderService", Version = "v1" });
});

builder.Services.AddOutputCache();

builder.Services.AddTransient<IOrderDataBaseContext, OrderDataBaseContext>();

builder.Services.Configure<RabbitMqConfiguration>(configuration.GetSection("RabbitMq"));

builder.Services.AddTransient<IOrderService, OrderService.Model.Services.OrderService.OrderService>();
builder.Services.AddTransient<IRegisterOrderService, RegisterOrderService>();
builder.Services.AddTransient<IProductService, ProductService>();
builder.Services.AddTransient<IVerifyProductService>(p =>
    new VerifyProductService(new RestClient(configuration["MicroServiceAddress:Product:Uri"])));

builder.Services.AddHostedService<ReceivedOrderCreatedMessage>();
builder.Services.AddHostedService<ReceivedPaymentOfOrderMessage>();
builder.Services.AddHostedService<ReceivedUpdateProductNameMessage>();
builder.Services.AddTransient<IMessageBus, RabbitMqMessageBus>();

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(option =>
    {
        option.Authority = configuration["Identity:Uri"];
        option.Audience = configuration["Identity:Audience"];
        option.RequireHttpsMetadata = false;
        option.TokenValidationParameters = new TokenValidationParameters()
        {
            ValidateIssuer = false
        };
    });

builder.Services.AddAuthorization(option =>
{
    option.AddPolicy(configuration["Identity:Scopes:OrdersManagement"], policy =>
    {
        policy.RequireClaim("scope", $"{configuration["Identity:Audience"]}.{configuration["Identity:Scopes:OrdersManagement"]}");    });
    option.AddPolicy(configuration["Identity:Scopes:GetOrders"], policy =>
    {
        policy.RequireClaim("scope",
            $"{configuration["Identity:Audience"]}.{configuration["Identity:Scopes:GetOrders"]}");
    });
});

var app = builder.Build();

app.MapDefaultEndpoints();

if (app.Environment.IsDevelopment() || app.Environment.IsProduction())
{
    app.UseDeveloperExceptionPage();
    app.UseSwagger();
    app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "OrderService v1"));
}

app.UseHttpsRedirection();

app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();

app.MapDefaultControllerRoute();

app.Run();