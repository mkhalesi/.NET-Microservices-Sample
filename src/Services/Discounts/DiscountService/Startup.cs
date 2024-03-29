using DiscountService.Infrastructure.Contexts;
using DiscountService.Infrastructure.MappingProfile;
using DiscountService.Model.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using DiscountService.GRPC;

namespace DiscountService
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
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "DiscountService", Version = "v1" });
            });

            services.AddDbContext<DiscountDataBaseContext>(o => o.UseSqlServer
                 (Configuration["DiscountConnection"]), ServiceLifetime.Singleton);

            services.AddGrpc();
            services.AddAutoMapper(typeof(DiscountMappingProfile));
            services.AddTransient<IDiscountService, DiscountService.Model.Services.DiscountService>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, DiscountDataBaseContext dbContext)
        {
            if (env.IsDevelopment() || env.IsProduction())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "DiscountService v1"));

                dbContext.Database.Migrate();
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapGrpcService<GRPCDiscountService>();
                endpoints.MapControllers();
            });
        }
    }
}
