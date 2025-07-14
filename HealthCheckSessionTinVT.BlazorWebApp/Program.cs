using HealthCheckSessionTinVT.BlazorWebApp.ASM2.Components;
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

// -------------------- 2. Cấu hình DbContext EF Core --------------------
builder.Services.AddDbContext<SU25_PRN222_SE1706_G1_SMMSContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"))
);

// -------------------- 3. Đăng ký các service & repository --------------------
builder.Services.AddScoped<UserAccountService>();
builder.Services.AddScoped<IHealthCheckSessionTinVTService, HealthCheckSessionTinVTService>();
builder.Services.AddScoped<HealthCheckStudentTinVtService>();
builder.Services.AddScoped<ISessionService, SessionService>();
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
builder.Services.AddScoped<HealthCheckStudentTinVtRepository>();
builder.Services.AddScoped<IServiceProviders, ServiceProviders>();

// -------------------- 4. Cấu hình Razor Components + MVC --------------------
builder.Services.AddRazorComponents().AddInteractiveServerComponents();
builder.Services.AddControllersWithViews();

// -------------------- 5. Build ứng dụng --------------------
var app = builder.Build();

// -------------------- 6. Middleware pipeline --------------------
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseSession();        // Quan trọng: gọi trước UseRouting
app.UseRouting();
app.UseAntiforgery();
app.UseAuthorization();

// -------------------- 7. Map endpoint --------------------
app.MapRazorComponents<App>().AddInteractiveServerRenderMode();

app.MapFallback(async context =>
{
    var sessionService = context.RequestServices.GetService<ISessionService>();
    if (sessionService != null && !sessionService.IsLoggedIn())
    {
        context.Response.Redirect("/account/login");
    }
    else
    {
        context.Response.Redirect("/");
    }
});

app.Run();