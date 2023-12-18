using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
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

namespace OrderService
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {

            services.AddControllers();
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "OrderService", Version = "v1" });
            });
            services.AddDbContext<OrderDataBaseContext>(o => o.UseSqlServer
                (Configuration["OrderConnection"]), ServiceLifetime.Singleton);

            services.Configure<RabbitMqConfiguration>(Configuration.GetSection("RabbitMq"));

            services.AddTransient<IOrderService, Model.Services.OrderService.OrderService>();
            services.AddTransient<IRegisterOrderService, RegisterOrderService>();
            services.AddTransient<IProductService, ProductService>();
            services.AddTransient<IVerifyProductService>(p =>
                new VerifyProductService(new RestClient(Configuration["MicroServiceAddress:Product:Uri"])));

            services.AddHostedService<ReceivedOrderCreatedMessage>();
            services.AddHostedService<ReceivedPaymentOfOrderMessage>();
            services.AddHostedService<ReceivedUpdateProductNameMessage>();
            services.AddTransient<IMessageBus, RabbitMqMessageBus>();

            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(option =>
                {
                    option.Authority = Configuration["Identity:Uri"];
                    option.Audience = Configuration["Identity:Audience"];
                    option.RequireHttpsMetadata = false;
                    option.TokenValidationParameters = new TokenValidationParameters()
                    {
                        ValidateIssuer = false
                    };
                });

            services.AddAuthorization(option =>
            {
                option.AddPolicy(Configuration["Identity:Scopes:OrdersManagement"], policy =>
                {
                    policy.RequireClaim("scope", $"{Configuration["Identity:Audience"]}.{Configuration["Identity:Scopes:OrdersManagement"]}");
                });
                option.AddPolicy(Configuration["Identity:Scopes:GetOrders"], policy =>
                {
                    policy.RequireClaim("scope", $"{Configuration["Identity:Audience"]}.{Configuration["Identity:Scopes:GetOrders"]}");
                });
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, OrderDataBaseContext dbContext)
        {
            if (env.IsDevelopment() && env.IsProduction())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "OrderService v1"));

                dbContext.Database.Migrate();
            }

            app.UseHttpsRedirection();

            app.UseRouting();
            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
