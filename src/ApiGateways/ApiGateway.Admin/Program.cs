using Microsoft.AspNetCore.Authentication.JwtBearer;
using Ocelot.DependencyInjection;
using Ocelot.Middleware;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

IConfiguration configuration = builder.Configuration;
IWebHostEnvironment webHostEnvironment = builder.Environment;
builder.Configuration.SetBasePath(webHostEnvironment.ContentRootPath)
    .AddJsonFile("ocelot.json")
    .AddOcelot(webHostEnvironment)
    .AddEnvironmentVariables();

var authenticationSchemeKey = "ApiGatewayAdminAuthenticationScheme";

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(authenticationSchemeKey, option =>
    {
        option.Authority = configuration["Identity:Uri"];
        option.Audience = configuration["Identity:Uri"];
    });

builder.Services.AddOcelot(configuration);

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.UseOcelot().Wait();

app.Run();
