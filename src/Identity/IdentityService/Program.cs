using Duende.IdentityServer.Models;
using Duende.IdentityServer.Test;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorPages();

builder.Services.AddIdentityServer()
    .AddDeveloperSigningCredential()
    .AddInMemoryIdentityResources(new List<IdentityResource>())
    .AddInMemoryClients(new List<Client>()
    {
        new Client()
        {
            ClientName = "Frontend Web",
            ClientId = "webFrontend",
            ClientSecrets = {new Secret("123321".Sha256())},
            AllowedGrantTypes = GrantTypes.ClientCredentials,
            AllowedScopes = { "OrderService.FullAccess" }
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
        new () { Name = "OrderService.FullAccess" }
    })
    .AddInMemoryApiResources(new List<ApiResource>()
    {
        new()
        {
            Name = "OrderService",
            Description = "OrderService Api",
            Scopes = { "OrderService.FullAccess" }
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
