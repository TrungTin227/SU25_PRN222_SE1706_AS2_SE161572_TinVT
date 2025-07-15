using HealthCheckSessionTinVT.BlazorWebApp.ASM2.Components;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.EntityFrameworkCore;
using SMMS.Repositories.TinVT;
using SMMS.Repositories.TinVT.Models;
using SMMS.Services.TinVT;

var builder = WebApplication.CreateBuilder(args);

// -------------------- 1. Cấu hình Session --------------------
builder.Services.AddDistributedMemoryCache();
builder.Services.AddSession(options =>
{
    options.Cookie.Name = ".SMMS.Session";
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
    options.IdleTimeout = TimeSpan.FromMinutes(30);
});
builder.Services.AddHttpContextAccessor();

// -------------------- 2. Cấu hình Authentication + Authorization --------------------
builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
    {
        options.LoginPath = "/account/login";
        options.LogoutPath = "/account/logout";
        options.AccessDeniedPath = "/access-denied";
        options.Cookie.HttpOnly = true;
        options.Cookie.SecurePolicy = CookieSecurePolicy.Always;
        options.Cookie.SameSite = SameSiteMode.Strict;
        options.ExpireTimeSpan = TimeSpan.FromHours(1);
    });

builder.Services.AddAuthorization();

// -------------------- 3. Cấu hình DbContext EF Core --------------------
builder.Services.AddDbContext<SU25_PRN222_SE1706_G1_SMMSContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"))
);

// -------------------- 4. Đăng ký các service & repository --------------------
builder.Services.AddScoped<UserAccountService>();
builder.Services.AddScoped<IHealthCheckSessionTinVTService, HealthCheckSessionTinVTService>();
builder.Services.AddScoped<HealthCheckStudentTinVtService>();
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
builder.Services.AddScoped<HealthCheckStudentTinVtRepository>();
builder.Services.AddScoped<IServiceProviders, ServiceProviders>();

// -------------------- 5. Cấu hình Razor Components + MVC --------------------
builder.Services.AddRazorComponents().AddInteractiveServerComponents();
builder.Services.AddControllersWithViews();

// -------------------- 6. Build ứng dụng --------------------
var app = builder.Build();

// -------------------- 7. Middleware Pipeline --------------------
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseSession();               // ✅ Duy trì session
app.UseAuthentication();        // ✅ Phải đặt trước Authorization
app.UseAuthorization();

app.UseAntiforgery();

// -------------------- 8. Map endpoint --------------------
app.MapRazorComponents<App>().AddInteractiveServerRenderMode();
app.MapControllers(); // Nếu có dùng controller cho API

// -------------------- 9. Chạy ứng dụng --------------------
app.Run();
