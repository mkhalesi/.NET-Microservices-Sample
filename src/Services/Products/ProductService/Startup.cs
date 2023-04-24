using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Protocols.OpenIdConnect;
using Microsoft.OpenApi.Models;
using ProductService.Infrastructure.Contexts;
using ProductService.MessagingBus.Config;
using ProductService.MessagingBus.SendMessages;
using ProductService.Model.Services.ProductCategoryService;
using ProductService.Model.Services.ProductService;

namespace ProductService
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
            services.AddSwaggerGen(c => // receive
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "ProductService", Version = "v1" });
            });

            services.AddDbContext<ProductDatabaseContext>(p =>
                p.UseSqlServer(Configuration["ProductConnection"]));

            services.Configure<RabbitMqConfiguration>(Configuration.GetSection("RabbitMq"));

            services.AddTransient<IProductService, Model.Services.ProductService.ProductService>();
            services.AddTransient<ICategoryService, CategoryService>();
            services.AddTransient<IMessageBus, RabbitMqMessageBus>();

            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(options =>
                {
                    options.Authority = "https://localhost:7017";
                    options.Audience = "ProductService";
                });

            services.AddAuthorization(options =>
            {
                options.AddPolicy("ProductsManagement",
                    policy => policy.RequireClaim("scope", "ProductService.ProductsManagement"));
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, ProductDatabaseContext dbContext)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "ProductService v1"));
            }

            dbContext.Database.Migrate();

            //using (var scope = app.Service.CraeteScope())
            //{
            //    var db = scope.ServiceProvider.GetRequiredService<ProductDatabaseContext>();
            //    db.Database.Migrate();
            //}

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
