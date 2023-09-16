using Microservices.Admin.Frontend.Models.ViewServices.ProductServices;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

ConfigurationManager Configuration = builder.Configuration;
IWebHostEnvironment Environment = builder.Environment;

builder.Services.AddHttpContextAccessor();

builder.Services.AddScoped<IProductManagementService>(p =>
    new ProductManagementService(new RestSharp.RestClient(Configuration["MicroserviceAddress:AdminApiGateway:Uri"]),
    new HttpContextAccessor()));

builder.Services.AddAuthentication(options =>
    {
        options.DefaultScheme = CookieAuthenticationDefaults.AuthenticationScheme;
        options.DefaultChallengeScheme = OpenIdConnectDefaults.AuthenticationScheme;
    }).AddCookie(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddOpenIdConnect(OpenIdConnectDefaults.AuthenticationScheme, options =>
    {
        options.SignInScheme = CookieAuthenticationDefaults.AuthenticationScheme;
        options.Authority = Configuration["Identity:Uri"];
        options.ClientId = "adminFrontendCode";
        options.ClientSecret = "123321";
        options.ResponseType = "code";
        options.SaveTokens = true;
        options.GetClaimsFromUserInfoEndpoint = true;

        //options.Scope.Add(OidcConstants.StandardScopes.OpenId);
        //options.Scope.Add(OidcConstants.StandardScopes.Profile);
        options.Scope.Add("openid");
        options.Scope.Add("profile");
        options.Scope.Add("ApiGatewayAdmin.FullAccess");
        options.Scope.Add("ProductService.ProductsManagement");

        options.Scope.Add("roles");
        options.ClaimActions.MapUniqueJsonKey("role", "role");
        options.TokenValidationParameters = new TokenValidationParameters()
        {
            NameClaimType = "name",
            RoleClaimType = "role"
        };
    });

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
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

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
