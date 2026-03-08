using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using StoreMaster.Core.Entities;
using StoreMaster.Core.Interfaces;
using StoreMaster.Core.Services;
using StoreMaster.Infrastructure.Data;
using StoreMaster.Infrastructure.Repositories;
using StoreMaster.Web.Mapping;

var builder = WebApplication.CreateBuilder(args);

// ── Servicios ────────────────────────────────────────────
builder.Services.AddControllersWithViews();

// Base de datos
builder.Services.AddDbContext<StoreDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// Repositorios
builder.Services.AddScoped<ICategoriaRepository, CategoriaRepository>();

// Servicios
builder.Services.AddScoped<ICategoriaService, CategoriaService>();

// Identity
builder.Services.AddIdentity<ApplicationUser, IdentityRole>(options =>
{
    // Requisitos de contraseña
    options.Password.RequireDigit = true;
    options.Password.RequiredLength = 6;
    options.Password.RequireUppercase = false;
    options.Password.RequireNonAlphanumeric = false;

    // Bloqueo de cuenta tras intentos fallidos
    options.Lockout.MaxFailedAccessAttempts = 5;
    options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(5);

    // El correo debe ser unico
    options.User.RequireUniqueEmail = true;
})
.AddEntityFrameworkStores<StoreDbContext>()
.AddDefaultTokenProviders();

// Configurar cookie de autenticacion
builder.Services.ConfigureApplicationCookie(options =>
{
    options.LoginPath = "/Account/Login";
    options.LogoutPath = "/Account/Logout";
    options.AccessDeniedPath = "/Account/AccessDenied";
    options.ExpireTimeSpan = TimeSpan.FromHours(8);
    options.SlidingExpiration = true;
});

// AutoMapper (busca perfiles en todos los ensamblados)
builder.Services.AddAutoMapper(cfg =>
{
    cfg.AddProfile<MappingProfile>();
});
var app = builder.Build();

// ── Pipeline HTTP ────────────────────────────────────────
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
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
// Crear roles y usuario admin al iniciar
await SeedDatabase(app);

app.Run();

// ── Seed inicial ─────────────────────────────────────────
static async Task SeedDatabase(WebApplication app)
{
    using var scope = app.Services.CreateScope();
    var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();
    var userManager = scope.ServiceProvider.GetRequiredService<UserManager<ApplicationUser>>();

    // Crear roles si no existen
    string[] roles = ["Admin", "Vendedor"];
    foreach (var role in roles)
    {
        if (!await roleManager.RoleExistsAsync(role))
            await roleManager.CreateAsync(new IdentityRole(role));
    }

    // Crear usuario Admin por defecto si no existe
    var adminEmail = "admin@storemaster.com";
    if (await userManager.FindByEmailAsync(adminEmail) == null)
    {
        var admin = new ApplicationUser
        {
            UserName = adminEmail,
            Email = adminEmail,
            Nombre = "Administrador",
            Apellido = "Sistema",
            Activo = true,
            EmailConfirmed = true
        };

        await userManager.CreateAsync(admin, "Admin123!");
        await userManager.AddToRoleAsync(admin, "Admin");
    }
}