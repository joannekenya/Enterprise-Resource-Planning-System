using ERP_SYSTEM.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// =========================
// MVC
// =========================
builder.Services.AddControllersWithViews();

// =========================
// DB CONTEXT
// =========================
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("Dev_Connection")));

// =========================
// PASSWORD HASHER
// =========================
builder.Services.AddScoped<IPasswordHasher<User>, PasswordHasher<User>>();

// =========================
// SESSION (IMPORTANT FIX)
// =========================
builder.Services.AddDistributedMemoryCache();

builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(30);
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
});

// =========================
// HTTP CONTEXT ACCESSOR
// =========================
builder.Services.AddHttpContextAccessor();

var app = builder.Build();

// =========================
// MIDDLEWARE PIPELINE
// =========================
app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

// 🔥 MUST BE BEFORE AUTHORIZATION
app.UseSession();

app.UseAuthorization();

// =========================
// ROUTES
// =========================
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();