//var builder = WebApplication.CreateBuilder(args);

//// Add services to the container.
//builder.Services.AddControllersWithViews();

//var app = builder.Build();

//// Configure the HTTP request pipeline.
//if (!app.Environment.IsDevelopment())
//{
//    app.UseExceptionHandler("/Home/Error");
//    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
//    app.UseHsts();
//}

//app.UseHttpsRedirection();
//app.UseStaticFiles();

//app.UseRouting();

//app.UseAuthorization();

//app.MapControllerRoute(
//    name: "default",
//    pattern: "{controller=Home}/{action=Index}/{id?}");

//app.Run();


using Comun.Dto;

using Microsoft.AspNetCore.Antiforgery;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.CookiePolicy;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;

using Negocio.Areas.ConsultasExternas;

using Negocio.Contratos.ConsultasExternas;
using ProyectoEcommers.Models;


var builder = WebApplication.CreateBuilder(args);

// Configuración de la base de datos (MySQL)
builder.Services.AddDbContext<ModelContext>(options =>
    options.UseMySQL(builder.Configuration.GetConnectionString("MySqlConnection"))); // o el proveedor de base de datos que uses

// Configuración de servicios
builder.Services.AddControllersWithViews();

// Configuración de autenticación con cookies
builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
    {
        options.LoginPath = "/Cuenta/Login";  // Redirige al login si no está autenticado
        options.AccessDeniedPath = "/Cuenta/CerrarSesion"; // Redirige a la página de error si no tiene acceso
        options.LogoutPath = "/Cuenta/CerrarSesion"; // Redirige al logout
        options.Cookie.Name = "Web";
        options.Cookie.HttpOnly = true;
        options.Cookie.SecurePolicy = CookieSecurePolicy.Always;  // Usar siempre cookies seguras
        options.Cookie.SameSite = SameSiteMode.Strict;  // Configura la política SameSite
        options.ExpireTimeSpan = TimeSpan.FromMinutes(30); // Sesión expira después de 30 minutos
        options.Cookie.MaxAge = options.ExpireTimeSpan;  // La cookie expirará después de 30 minutos
        options.SlidingExpiration = true;  // Permite la expiración deslizante (renovación de la cookie)
        options.ReturnUrlParameter = CookieAuthenticationDefaults.ReturnUrlParameter;
        options.Cookie.IsEssential = true;  // Hace la cookie esencial para la operación
    });

// Configuración de CORS
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowMercadoPago", builder =>
    {
        builder.WithOrigins("https://www.mariabonita.com.co/")
               .AllowAnyHeader()
               .AllowAnyMethod()
               .AllowCredentials(); // Permitir credenciales
    });
});

// Configuración de Anti-forgery
builder.Services.AddAntiforgery(options =>
{
    options.HeaderName = "X-XSRF-TOKEN"; // Nombre del encabezado para el token
    options.Cookie.SecurePolicy = CookieSecurePolicy.Always; // Siempre Secure
});

builder.Services.Configure<AntiforgeryOptions>(options =>
{
    options.Cookie.HttpOnly = true;
    options.Cookie.SecurePolicy = CookieSecurePolicy.Always;
    options.Cookie.SameSite = SameSiteMode.Strict;
});

// Configuración de Cookie Policy
builder.Services.Configure<CookiePolicyOptions>(options =>
{
    options.MinimumSameSitePolicy = SameSiteMode.Lax;
    options.HttpOnly = HttpOnlyPolicy.Always;
    options.OnAppendCookie = cookieContext =>
    {
        cookieContext.CookieOptions.Secure = true; // Asegúrate de que las cookies sean seguras
    };
    options.OnDeleteCookie = cookieContext =>
    {
        cookieContext.CookieOptions.Secure = true; // Asegúrate de que las cookies sean seguras
    };
});

// Configuración de sesión
builder.Services.AddSession(options =>
{
    options.Cookie.HttpOnly = true; // Previene el acceso del lado del cliente
    options.Cookie.IsEssential = true; // Hace que la cookie sea esencial para el funcionamiento
    options.Cookie.SecurePolicy = CookieSecurePolicy.Always; // Configura las cookies como seguras (solo HTTPS)
    options.Cookie.SameSite = SameSiteMode.Strict; // Configura SameSite para evitar el envío en solicitudes cruzadas
});

// Agregar HTTP Client
builder.Services.AddHttpClient();

// Cargar la configuración de ApiGatewayUrl desde appsettings.json
ApiGatewayUrl apiGateWay = new ApiGatewayUrl();
apiGateWay.CarruselImagenes = builder.Configuration.GetValue<string>("CarruselImagenes");
builder.Services.AddSingleton(apiGateWay);

builder.Services.AddHttpContextAccessor();

// Inyección de dependencias
builder.Services.AddTransient<ModelContext>();
builder.Services.AddScoped<IDbAdministracion, DbAdministracion>();
builder.Services.AddScoped<IDbProducto, DBProducto>();
builder.Services.AddScoped<IBLConsultasExternas, BLConsultasExternas>();
builder.Services.AddScoped<IApiExternos, ApiExternos>();
builder.Services.AddTransient<IDbSlider, DBSlider>();
builder.Services.AddTransient<IDbComentarios, DbComentarios>();

// Variables de sesión
builder.Services.AddMvc();
builder.Services.AddDistributedMemoryCache();
builder.Services.AddSession();

// Configuración de AppSettings
builder.Services.AddOptions();
builder.Services.Configure<AppSettings>(builder.Configuration.GetSection("AppSettings"));

var RutaVisualizador = builder.Configuration.GetValue<string>("Visualizador");
builder.Services.AddSignalR();

var app = builder.Build();

// Middleware para la sesión
app.UseSession();

// Configuración de entorno
if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
}
else
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

// Middleware de CORS
app.UseCors("AllowMercadoPago");

// Autenticación y autorización
app.UseAuthentication();
app.UseAuthorization();

// Seguridad de encabezados (opcional)
app.Use(async (context, next) =>
{
    context.Response.Headers["X-Frame-Options"] = "DENY";
    context.Response.Headers["X-Content-Type-Options"] = "nosniff";
    context.Response.Headers["Referrer-Policy"] = "no-referrer-when-downgrade"; // Ajustado

    await next();
});

// Configuración de rutas de controlador
app.MapControllerRoute(
    name: "areas",
    pattern: "{area:exists}/{controller=Home}/{action=Index}/{id?}");

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Cuenta}/{action=Index}/{id?}");

// Ruta para cerrar sesión (Eliminar la cookie de autenticación)
app.MapControllerRoute(
    name: "CerrarSesion",
    pattern: "/Cuenta/CerrarSesion",
    defaults: new { controller = "Cuenta", action = "Login" }
);

app.Run();


