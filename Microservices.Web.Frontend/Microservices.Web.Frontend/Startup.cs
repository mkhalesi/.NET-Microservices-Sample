using Microservices.Web.Frontend.Services.BasketServices;
using Microservices.Web.Frontend.Services.DiscountService;
using Microservices.Web.Frontend.Services.OrderServices;
using Microservices.Web.Frontend.Services.PaymentServices;
using Microservices.Web.Frontend.Services.ProductServices;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using RestSharp;

namespace Microservices.Web.Frontend
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
            services.AddControllersWithViews();

            services.AddScoped<IDiscountService, Services.DiscountService.DiscountService>();

            services.AddScoped<IProductService>(p => new ProductService(
                new RestClient(Configuration["MicroserviceAddress:Product:Uri"])));

            services.AddScoped<IBasketService>(p => new BasketServices(
                new RestClient(Configuration["MicroserviceAddress:Basket:Uri"])));

            services.AddScoped<IOrderService>(p => new OrderService(
                new RestClient(Configuration["MicroserviceAddress:Order:Uri"])));

            services.AddScoped<IPaymentService>(p => new PaymentService(
                new RestClient(Configuration["MicroserviceAddress:Payment:Uri"])));
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }
            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}
