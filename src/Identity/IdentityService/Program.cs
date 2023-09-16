using Duende.IdentityServer;
using Duende.IdentityServer.Models;
using IdentityService.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddIdentity<IdentityUser, IdentityRole>()
    .AddEntityFrameworkStores<ApplicationDbContext>()
    .AddDefaultTokenProviders();

// Add services to the container.
builder.Services.AddRazorPages();

IConfiguration configuration = builder.Configuration;
builder.Services.AddDbContext<ApplicationDbContext>(option =>
    option.UseSqlServer(configuration["AspIdentityConnection"]));

builder.Services.AddIdentityServer()
    .AddDeveloperSigningCredential()
    .AddInMemoryIdentityResources(new List<IdentityResource>()
    {
        new IdentityResources.OpenId(),
        new IdentityResources.Profile(),
        new("roles", "User role(s)", new List<string> { "role" })
    })
    .AddInMemoryClients(new List<Client>()
    {
        //new ()
        //{
        //    ClientName = "Web Frontend",
        //    ClientId = "webFrontend",
        //    ClientSecrets = {new Secret("123321".Sha256())},
        //    AllowedGrantTypes = GrantTypes.ClientCredentials,
        //    AllowedScopes = { "OrderService.FullAccess" }
        //},
        new()
        {
            ClientName = "Web Frontend Code",
            ClientId = "webFrontendCode",
            ClientSecrets = { new Secret("123321".Sha256()) },
            AllowedGrantTypes = GrantTypes.Code,
            RedirectUris = { $"{configuration["WebFrontend:Uri"]}/signin-oidc" },
            PostLogoutRedirectUris = { $"{configuration["WebFrontend:Uri"]}/signout-oidc" },
            AllowedScopes =
            {
                IdentityServerConstants.StandardScopes.OpenId,
                IdentityServerConstants.StandardScopes.Profile,
                "OrderService.GetOrders",
                "BasketService.FullAccess",
                "ApiGatewayForWeb.FullAccess",
                "roles"
            },
            //for refresh token
            AllowOfflineAccess = true,
            AccessTokenLifetime = 3600,
            RefreshTokenUsage = TokenUsage.ReUse,
            RefreshTokenExpiration = TokenExpiration.Sliding
        },
        new()
        {
            ClientName = "Admin Frontend Code",
            ClientId = "adminFrontendCode",
            ClientSecrets = { new Secret("123321".Sha256()) },
            AllowedGrantTypes = GrantTypes.Code,
            RedirectUris = { $"{configuration["AdminFrontend:Uri"]}/signin-oidc" },
            PostLogoutRedirectUris= { $"{configuration["AdminFrontend:Uri"]}/signout-oidc" },
            AllowedScopes =
            {
                IdentityServerConstants.StandardScopes.OpenId,
                IdentityServerConstants.StandardScopes.Profile,
                "OrderService.GetOrders",
                "OrderService.OrdersManagement",
                "ApiGatewayAdmin.FullAccess",
                "ProductService.ProductsManagement",
                "roles"
            },
        }
    })
    //.AddTestUsers(new List<TestUser>()
    //{
    //    new()
    //    {
    //        IsActive = true,
    //        Password = "123321",
    //        Username = "admin",
    //        SubjectId = "TestGuid"
    //    }
    //})
    .AddInMemoryApiScopes(new List<ApiScope>()
    {
        new() { Name = "OrderService.GetOrders" },
        new() { Name = "OrderService.OrdersManagement" },
        new() { Name = "BasketService.FullAccess" },
        new() { Name = "ApiGatewayForWeb.FullAccess", UserClaims = new List<string> { "role" } },
        new() { Name = "ApiGatewayAdmin.FullAccess", UserClaims = new List<string> { "role" } },
        new() { Name = "ProductService.ProductsManagement" }
    })
    .AddInMemoryApiResources(new List<ApiResource>()
    {
        new()
        {
            Name = "OrderService",
            Description = "OrderService Api",
            Scopes = { "OrderService.GetOrders", "OrderService.OrdersManagement" }
        },
        new()
        {
            Name = "BasketService",
            Description = "BasketService Api",
            Scopes = { "BasketService.FullAccess" }
        },
        new()
        {
            Name = "ApiGatewayForWeb",
            Description = "ApiGatewayForWeb",
            Scopes = { "ApiGatewayForWeb.FullAccess" }
        },
        new()
        {
            Name = "ApiGatewayAdmin",
            Description = "ApiGateway For Admin",
            Scopes = { "ApiGatewayAdmin.FullAccess" }
        },
        new()
        {
            Name = "ProductService",
            Description = "ProductService",
            Scopes = { "ProductService.ProductsManagement" }
        }
    })
    .AddAspNetIdentity<IdentityUser>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    //app.UseHsts();
    SeedUserData.Seed(app);
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();
app.UseIdentityServer();
app.UseAuthorization();

app.MapRazorPages();

app.Run();
