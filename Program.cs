using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Vaperia_drink.Components;
using Vaperia_drink.Components.Account;
using Vaperia_drink.Data;

var builder = WebApplication.CreateBuilder(args);

// 🧩 Blazor Server
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();

// 🧩 Autenticación e identidad
builder.Services.AddCascadingAuthenticationState();
builder.Services.AddScoped<IdentityUserAccessor>();
builder.Services.AddScoped<IdentityRedirectManager>();
builder.Services.AddScoped<AuthenticationStateProvider, IdentityRevalidatingAuthenticationStateProvider>();

builder.Services.AddAuthentication(options =>
{
    options.DefaultScheme = IdentityConstants.ApplicationScheme;
    options.DefaultSignInScheme = IdentityConstants.ExternalScheme;
})
.AddIdentityCookies();

// 🧩 Base de datos SQLite
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection")
    ?? "Data Source=VaperiaDb.db";

builder.Services.AddDbContextFactory<ApplicationDbContext>(options =>
    options.UseSqlite(connectionString));

builder.Services.AddDatabaseDeveloperPageExceptionFilter();

// 🧩 Configuración de Identity
builder.Services.AddIdentityCore<ApplicationUser>(options =>
{
    options.SignIn.RequireConfirmedAccount = false; // Puedes ponerlo en true si usarás confirmación de correo
})
.AddEntityFrameworkStores<ApplicationDbContext>()
.AddSignInManager()
.AddDefaultTokenProviders();



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
app.UseAntiforgery();

// 🧩 Mapeo de componentes y rutas
app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();

// 🧩 Endpoints de Identity
app.MapAdditionalIdentityEndpoints();

app.Run();
