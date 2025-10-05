using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Vaperia_drink.Components;
using Vaperia_drink.Components.Account;
using Vaperia_drink.Data;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Components.Server;

var builder = WebApplication.CreateBuilder(args);

// 🧩 Blazor Server
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();

// 🧩 Base de datos SQLite
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection")
    ?? "Data Source=VaperiaDb.db";

builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlite(connectionString));

builder.Services.AddDatabaseDeveloperPageExceptionFilter();

// 🧩 Configuración completa de Identity
builder.Services.AddIdentity<ApplicationUser, IdentityRole>(options =>
{
    options.SignIn.RequireConfirmedAccount = false; // Cambiar a true si usarás confirmación de correo
})
.AddEntityFrameworkStores<ApplicationDbContext>()
.AddDefaultTokenProviders();

// 🧩 AuthenticationStateProvider para Blazor (usando el estándar)
builder.Services.AddScoped<AuthenticationStateProvider, ServerAuthenticationStateProvider>();
builder.Services.AddScoped<IdentityRedirectManager>();
builder.Services.AddScoped<IdentityUserAccessor>();

// 🧩 Configuración de cookies
builder.Services.ConfigureApplicationCookie(options =>
{
    options.LoginPath = "/Account/Login";
    options.LogoutPath = "/Account/Logout";
});

// 🧩 Email sender (puede ser no-op o real)
builder.Services.AddSingleton<IEmailSender<ApplicationUser>, IdentityNoOpEmailSender>();

// 🧩 Bootstrap para Blazor
builder.Services.AddBlazorBootstrap();

var app = builder.Build();

// 🧩 Middleware HTTP
if (app.Environment.IsDevelopment())
{
    app.UseMigrationsEndPoint();
}
else
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();
app.UseAntiforgery();

// 🧩 Mapeo de componentes y rutas
app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();

// 🧩 Endpoints de Identity
app.MapAdditionalIdentityEndpoints();

app.Run();
