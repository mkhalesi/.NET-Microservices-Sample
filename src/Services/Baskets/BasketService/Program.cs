using BasketService.Infrastructure.Contexts;
using BasketService.Infrastructure.MappingProfile;
using BasketService.MessageBus.Config;
using BasketService.MessageBus.ReceivedMessages.ProductMessages;
using BasketService.MessageBus.SendMessages;
using BasketService.Model.Services.BasketService;
using BasketService.Model.Services.CacheService;
using BasketService.Model.Services.DiscountService;
using BasketService.Model.Services.ProductService;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;


var builder = WebApplication.CreateBuilder(args);

builder.AddServiceDefaults();

IConfiguration configuration = builder.Configuration;

builder.Services.AddControllers();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "OrderService", Version = "v1" });
});

builder.Services.AddOutputCache();

builder.Services.AddDbContext<BasketDataBaseContext>(o =>
    o.UseSqlServer(configuration["BasketConnection"]), ServiceLifetime.Singleton);

builder.Services.AddStackExchangeRedisCache(options =>
{
    options.Configuration = configuration["CacheSettings:ConnectionString"];
});

builder.Services.AddAutoMapper(typeof(BasketMappingProfile));
builder.Services.Configure<RabbitMqConfiguration>(configuration.GetSection("RabbitMq"));

builder.Services.AddTransient<IBasketService, BasketService.Model.Services.BasketService.BasketService>();
builder.Services.AddTransient<IDiscountService, BasketService.Model.Services.DiscountService.DiscountService>();
builder.Services.AddTransient<IProductService, ProductService>();
builder.Services.AddTransient<ICacheService, CacheService>();

builder.Services.AddHostedService<ReceivedUpdateProductNameMessage>();
builder.Services.AddTransient<IMessageBus, RabbitMqMessageBus>();

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.Authority = configuration["Identity:Uri"];
        options.Audience = configuration["Identity:Audience"];
        options.RequireHttpsMetadata = false;
        options.TokenValidationParameters = new TokenValidationParameters()
        {
            ValidateIssuer = false
        };
    });


var app = builder.Build();

app.MapDefaultEndpoints();

if (app.Environment.IsDevelopment() || app.Environment.IsProduction())
{
    app.UseDeveloperExceptionPage();
    app.UseSwagger();
    app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "BasketService v1"));

    using var scope = app.Services.CreateScope();
    var db = scope.ServiceProvider.GetRequiredService<BasketDataBaseContext>();
    db.Database.Migrate();
}

app.UseHttpsRedirection();

app.UseCookiePolicy(new CookiePolicyOptions { MinimumSameSitePolicy = SameSiteMode.Lax });

app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();

app.MapDefaultControllerRoute();

app.Run();