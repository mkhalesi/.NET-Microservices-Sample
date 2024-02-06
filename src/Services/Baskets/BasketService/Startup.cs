using BasketService.Infrastructure.Contexts;
using BasketService.Infrastructure.MappingProfile;
using BasketService.MessageBus.Config;
using BasketService.MessageBus.ReceivedMessages.ProductMessages;
using BasketService.MessageBus.SendMessages;
using BasketService.Model.Services.BasketService;
using BasketService.Model.Services.DiscountService;
using BasketService.Model.Services.ProductService;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;

namespace BasketService
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
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "BasketService", Version = "v1" });
            });

            services.AddDbContext<BasketDataBaseContext>(o =>
                o.UseSqlServer(Configuration["BasketConnection"]), ServiceLifetime.Singleton);

            services.AddStackExchangeRedisCache(options =>
            {
                options.Configuration = Configuration["CacheSettings:ConnectionString"];
            });

            services.AddAutoMapper(typeof(BasketMappingProfile));
            services.Configure<RabbitMqConfiguration>(Configuration.GetSection("RabbitMq"));

            services.AddTransient<IBasketService, Model.Services.BasketService.BasketService>();
            services.AddTransient<IDiscountService, Model.Services.DiscountService.DiscountService>();
            services.AddTransient<IProductService, ProductService>();

            services.AddHostedService<ReceivedUpdateProductNameMessage>();
            services.AddTransient<IMessageBus, RabbitMqMessageBus>();

            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(options =>
                {
                    options.Authority = Configuration["Identity:Uri"];
                    options.Audience = Configuration["Identity:Audience"];
                    options.RequireHttpsMetadata = false;
                    options.TokenValidationParameters = new TokenValidationParameters()
                    {
                        ValidateIssuer = false
                    };
                });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, BasketDataBaseContext dbContext)
        {
            if (env.IsDevelopment() || env.IsProduction())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "BasketService v1"));

                dbContext.Database.Migrate();
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
        }
    }
}
