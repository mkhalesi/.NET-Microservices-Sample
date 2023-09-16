using System;
using IdentityModel;
using Microservices.Web.Frontend.Services.BasketServices;
using Microservices.Web.Frontend.Services.DiscountService;
using Microservices.Web.Frontend.Services.OrderServices;
using Microservices.Web.Frontend.Services.PaymentServices;
using Microservices.Web.Frontend.Services.ProductServices;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
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
        public Startup(IConfiguration configuration, IHostEnvironment environment)
        {
            Configuration = configuration;
            this.environment = environment;
        }

        public IConfiguration Configuration { get; }
        private readonly IHostEnvironment environment;

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            var mvcService = services.AddControllersWithViews();
            services.AddHttpContextAccessor();
            services.AddAccessTokenManagement();

            //if (environment.IsDevelopment())
            //    mvcService.AddRazorRuntimeCompilation();

            //services.AddScoped<IDiscountService, Services.DiscountService.DiscountService>();
            services.AddScoped<IDiscountService>(p => new DiscountServiceRestful(
                new RestClient(Configuration["MicroserviceAddress:ApiGatewayForWeb:Uri"])));

            services.AddScoped<IProductService>(p => new ProductService(
                new RestClient(Configuration["MicroserviceAddress:ApiGatewayForWeb:Uri"])));

            services.AddScoped<IBasketService>(p => new BasketServices(
                new RestClient(Configuration["MicroserviceAddress:ApiGatewayForWeb:Uri"])));

            //services.AddScoped<IOrderService>(p => new OrderService(
            //    new RestClient("https://localhost:7001"), new HttpContextAccessor()));
            //services.AddScoped<IOrderService>(p => new OrderService(
            //    new RestClient(Configuration["MicroserviceAddress:ApiGatewayForWeb:Uri"]), new HttpContextAccessor()));
            services.AddHttpClient<IOrderService, OrderService>(p => 
                p.BaseAddress = new Uri(Configuration["MicroserviceAddress:ApiGatewayForWeb:Uri"])
            ).AddUserAccessTokenHandler();
            
            services.AddScoped<IPaymentService>(p => new PaymentService(
                new RestClient(Configuration["MicroserviceAddress:ApiGatewayForWeb:Uri"])));

            services.AddAuthentication(option =>
            {
                option.DefaultScheme = CookieAuthenticationDefaults.AuthenticationScheme;
                option.DefaultChallengeScheme = OpenIdConnectDefaults.AuthenticationScheme;

            }).AddCookie(CookieAuthenticationDefaults.AuthenticationScheme)
                .AddOpenIdConnect(OpenIdConnectDefaults.AuthenticationScheme, options =>
                {
                    options.SignInScheme = CookieAuthenticationDefaults.AuthenticationScheme;
                    options.Authority = Configuration["Identity:Uri"];
                    options.ClientId = "webFrontendCode";
                    options.ClientSecret = "123321";
                    options.ResponseType = "code";
                    options.GetClaimsFromUserInfoEndpoint = true;
                    options.SaveTokens = true;

                    // default
                    options.Scope.Add(OidcConstants.StandardScopes.OpenId);
                    options.Scope.Add(OidcConstants.StandardScopes.Profile);
                    options.Scope.Add("OrderService.GetOrders");
                    options.Scope.Add("BasketService.FullAccess");
                    options.Scope.Add("ApiGatewayForWeb.FullAccess");
                    // for refresh token
                    options.Scope.Add("offline_access");
                });
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
            app.UseAuthentication();
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
