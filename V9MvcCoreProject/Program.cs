using Cryptography.Utilities;
using DinkToPdf;
using DinkToPdf.Contracts;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using V9MvcCoreProject.DataDbContext;
using V9MvcCoreProject.Entities.Models;
using V9MvcCoreProject.Extensions;
using V9MvcCoreProject.Extensions.LogsHelpers.Interface;
using V9MvcCoreProject.Extensions.LogsHelpers.Service;
using V9MvcCoreProject.Middleware;
using V9MvcCoreProject.Middleware.PermissionAttribute;
using V9MvcCoreProject.Repository.Interface;
using V9MvcCoreProject.Repository.Services;

var builder = WebApplication.CreateBuilder(args);
var configuration = builder.Configuration;

// ===========================================================
// 1️⃣ CONFIGURATION & LOGGING
// ===========================================================
builder.Logging.ClearProviders();
builder.Logging.AddConsole(); // You can add Serilog here later if needed

// ===========================================================
// 2️⃣ DATABASE CONTEXT
// ===========================================================
builder.Services.AddDbContext<ApplicationDbContext>(options =>
{
    var decryptedConnection = new AesGcmEncryption(builder.Configuration)
        .Decrypt(builder.Configuration.GetConnectionString("DefaultConnection") ?? throw new InvalidOperationException("Connection String is missing."));

    options.UseSqlServer(decryptedConnection, sql =>
    {
        sql.CommandTimeout((int)TimeSpan.FromMinutes(1).TotalSeconds);
        //sql.EnableRetryOnFailure(
        //    maxRetryCount: 5,
        //    maxRetryDelay: TimeSpan.FromSeconds(15),
        //    errorNumbersToAdd: null
        //);
    });

    options.EnableDetailedErrors(false);
    options.EnableSensitiveDataLogging(false);
});

// Allow synchronous I/O (required by some legacy libs)
builder.Services.Configure<IISServerOptions>(options => options.AllowSynchronousIO = true);

// ===========================================================
// 3️⃣ IDENTITY & AUTHENTICATION
// ===========================================================
builder.Services.AddIdentity<ApplicationUser, ApplicationRole>(options =>
{
    options.User.AllowedUserNameCharacters = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789-._@+/ ";
    options.Password.RequiredLength = 5;
    options.Password.RequireLowercase = false;
    options.Password.RequireUppercase = false;
    options.Password.RequireNonAlphanumeric = false;
    options.Password.RequireDigit = false;
    //options.User.RequireUniqueEmail = false;
    //options.Password.RequireDigit = false;
    //options.Password.RequireNonAlphanumeric = false;
    //options.Password.RequireUppercase = false;
    //options.Password.RequireLowercase = false;
    // options.UserLockoutEnabledByDefault = true;
    //options.User.RequireUniqueEmail = true;
    //options.Password.RequireDigit = false;
    //options.Password.RequiredLength = 6;
    //options.Password.RequireNonAlphanumeric = false;
    //options.Password.RequireUppercase = false; 
    //options.Password.RequireLowercase = false; 
    ////options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromDays(360 * Int32.Parse(Configuration["PasswordPolicy:DefaultLockoutTimeSpan"]));
    ///////options.Lockout.MaxFailedAccessAttempts = Int32.Parse(Configuration["PasswordPolicy:MaxFailedAccessAttempts"]);
    ///////options.UserLockoutEnabledByDefault = true;
})
.AddEntityFrameworkStores<ApplicationDbContext>()
.AddDefaultTokenProviders();

builder.Services.ConfigureApplicationCookie(options =>
{
    options.LoginPath = "/Account/Login";
    options.LogoutPath = "/Account/LogOut";
    options.AccessDeniedPath = "/Account/AccessDenied";
    options.SlidingExpiration = true;
    options.ExpireTimeSpan = TimeSpan.FromHours(4);
});

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = CookieAuthenticationDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = CookieAuthenticationDefaults.AuthenticationScheme;
})
.AddCookie(options =>
{
    options.Cookie.HttpOnly = true;
    options.Cookie.SameSite = SameSiteMode.Strict;
    options.Cookie.SecurePolicy = CookieSecurePolicy.SameAsRequest;
    options.ExpireTimeSpan = TimeSpan.FromMinutes(30);
    options.SlidingExpiration = true;
})
.AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = builder.Configuration["Jwt:Issuer"],
        ValidAudience = builder.Configuration["Jwt:Audience"],
        IssuerSigningKey = new SymmetricSecurityKey(
            Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Secret"] ?? ""))
    };
});

// ===========================================================
// 4️⃣ CONTROLLERS + MVC
// ===========================================================
builder.Services.AddControllersWithViews(options =>
{
    var policy = new AuthorizationPolicyBuilder()
        .RequireAuthenticatedUser()
        .Build();
    options.Filters.Add(new AuthorizeFilter(policy));
})
.AddJsonOptions(o =>
{
    o.JsonSerializerOptions.PropertyNamingPolicy = null;
    o.JsonSerializerOptions.DictionaryKeyPolicy = null;
    o.JsonSerializerOptions.ReferenceHandler =
        System.Text.Json.Serialization.ReferenceHandler.IgnoreCycles;
})
.AddRazorRuntimeCompilation();

// For XML API endpoints if ever needed
builder.Services.AddControllers().AddXmlSerializerFormatters();

// ===========================================================
// 5️⃣ DEPENDENCY INJECTION
// ===========================================================
builder.Services.AddSingleton<AesGcmEncryption>();
builder.Services.AddScoped<CheckUserPermission>();
builder.Services.TryAddSingleton<IHttpContextAccessor, HttpContextAccessor>();

builder.Services.AddScoped<IUserServiceLayer, UserServiceLayer>();
builder.Services.AddScoped<IUserAccessServiceLayer, UserAccessServiceLayer>();
builder.Services.AddScoped<ILogService, LogService>();
builder.Services.AddScoped<ILoggerManager, LoggerManager>();

// PDF Generation (wkhtmltopdf)
var pdfLibPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "libwkhtmltox.dll");
var context = new CustomAssemblyLoadContext();
context.LoadUnmanagedLibrary(pdfLibPath);
builder.Services.AddSingleton<IConverter>(new SynchronizedConverter(new PdfTools()));

// ===========================================================
// 6️⃣ SESSION & CACHING
// ===========================================================
builder.Services.AddMemoryCache();
builder.Services.AddDistributedMemoryCache();

builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(30);
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
});

// ===========================================================
// 7️⃣ COOKIE POLICY
// ===========================================================
builder.Services.Configure<CookiePolicyOptions>(options =>
{
    options.CheckConsentNeeded = _ => true;
    options.MinimumSameSitePolicy = SameSiteMode.None;
});

// ===========================================================
// 8️⃣ PERFORMANCE (Optional, Recommended in Production)
// ===========================================================
//builder.Services.AddResponseCompression(options =>
//{
//    options.EnableForHttps = true;
//});

// ===========================================================
// 9️⃣ BUILD APP
// ===========================================================
var app = builder.Build();

// ===========================================================
// 🔟 MIDDLEWARE PIPELINE — ORDER MATTERS!
// ===========================================================

// Global exception handler (before HSTS/HTTPS)
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

app.UseSession();

// Add request/response logging middleware (non-blocking)
app.UseMiddleware<LogsMiddleware>();

//app.UseResponseCompression(); // Uncomment if enabled above

// MVC routes
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Account}/{action=Login}/{id?}");

app.Run();