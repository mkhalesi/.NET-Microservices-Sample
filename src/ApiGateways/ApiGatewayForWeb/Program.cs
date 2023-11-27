using ApiGateway.ForWeb.Models.DiscountServices;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Ocelot.Cache.CacheManager;
using Ocelot.DependencyInjection;
using Ocelot.Middleware;
using Ocelot.Provider.Polly;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddTransient<IDiscountService, ApiGateway.ForWeb.Models.DiscountServices.DiscountService>();

IConfiguration configuration = builder.Configuration;
IWebHostEnvironment environment = builder.Environment;

builder.Configuration.SetBasePath(environment.ContentRootPath)
    .AddJsonFile("ocelot.json")
    .AddOcelot(environment)
    .AddEnvironmentVariables();

//var authenticationSchemeKey = "ApiGatewayForWebAuthenticationScheme";

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(JwtBearerDefaults.AuthenticationScheme, option =>
    {
        option.Authority = configuration["Identity:Uri"];
        option.Audience = configuration["Identity:Audience"];
        option.RequireHttpsMetadata = false;
        option.TokenValidationParameters = new TokenValidationParameters()
        {
            ValidateIssuer = false
        };
    });

builder.Services
    .AddOcelot(configuration)
    .AddPolly()
    .AddCacheManager(x =>
    {
        x.WithDictionaryHandle();
    });
//builder.Services.ConfigureDownstreamHostAndPortsPlaceholders(configuration);

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseCookiePolicy(new CookiePolicyOptions { MinimumSameSitePolicy = SameSiteMode.Lax });

app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();

app.UseEndpoints(endpoints =>
{
    endpoints.MapControllers();
});
app.UseOcelot().Wait();

app.Run();
