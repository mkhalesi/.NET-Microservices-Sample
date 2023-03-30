using Duende.IdentityServer;
using Duende.IdentityServer.Models;
using Duende.IdentityServer.Test;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorPages();

builder.Services.AddIdentityServer()
    .AddDeveloperSigningCredential()
    .AddInMemoryIdentityResources(new List<IdentityResource>()
    {
        new IdentityResources.OpenId(),
        new IdentityResources.Profile()
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
        new ()
        {
            ClientName = "Web Frontend Code",
            ClientId = "webFrontendCode",
            ClientSecrets = {new Secret("123321".Sha256())},
            AllowedGrantTypes = GrantTypes.Code,
            RedirectUris = { "https://localhost:44327/signin-oidc" },
            PostLogoutRedirectUris = { "https://localhost:44327/signout-oidc" },
            AllowedScopes = {
                IdentityServerConstants.StandardScopes.OpenId,
                IdentityServerConstants.StandardScopes.Profile,
                "OrderService.GetOrders",
                "BasketService.FullAccess",
                "ApiGatewayForWeb.FullAccess"
            },
        },
        new ()
        {
            ClientName = "Admin Frontend Code",
            ClientId = "adminFrontendCode",
            ClientSecrets = {new Secret("123321".Sha256())},
            AllowedGrantTypes = GrantTypes.Code,
            RedirectUris = { "https://localhost:7297/signin-oidc" },
            PostLogoutRedirectUris = { "https://localhost:7297/signout-oidc" },
            AllowedScopes = {
                IdentityServerConstants.StandardScopes.OpenId,
                IdentityServerConstants.StandardScopes.Profile,
                "OrderService.GetOrders",
                "OrderService.OrdersManagement"
            },
        }
    })
    .AddTestUsers(new List<TestUser>()
    {
        new ()
        {
            IsActive = true,
            Password = "123321",
            Username = "admin",
            SubjectId = "TestGuid"
        }
    })
    .AddInMemoryApiScopes(new List<ApiScope>()
    {
        new () { Name = "OrderService.GetOrders" },
        new () { Name = "OrderService.OrdersManagement" },
        new () { Name = "BasketService.FullAccess" },
        new () { Name = "ApiGatewayForWeb.FullAccess" }
    })
    .AddInMemoryApiResources(new List<ApiResource>()
    {
        new()
        {
            Name = "OrderService",
            Description = "OrderService Api",
            Scopes = { "OrderService.GetOrders" , "OrderService.OrdersManagement" }
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
        }
    });

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();
app.UseIdentityServer();
app.UseAuthorization();

app.MapRazorPages();

app.Run();
