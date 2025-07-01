using HealthCheckSessionTinVT.BlazorWebApp.Components;
using Microsoft.EntityFrameworkCore;
using SMMS.Repositories.TinVT;
using SMMS.Repositories.TinVT.Models;
using SMMS.Services.TinVT;

var builder = WebApplication.CreateBuilder(args);

// 1. Cấu hình Session
builder.Services.AddDistributedMemoryCache();
builder.Services.AddSession(options =>
{
    options.Cookie.Name = ".SMMS.Session";
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
    options.IdleTimeout = TimeSpan.FromMinutes(30);
});
builder.Services.AddHttpContextAccessor();

// 2. Đăng ký EF Core DbContext
builder.Services.AddDbContext<SU25_PRN222_SE1706_G1_SMMSContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"))
);

// 3. Đăng ký các service & repository
builder.Services.AddScoped<IServiceProviders, ServiceProviders>();
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
builder.Services.AddScoped<IHealthCheckSessionTinVTService, HealthCheckSessionTinVTService>();
builder.Services.AddScoped<UserAccountService>();
builder.Services.AddScoped<HealthCheckStudentTinVtService>();
builder.Services.AddScoped<HealthCheckStudentTinVtRepository>();
builder.Services.AddScoped<ISessionService, SessionService>();

// 4. Blazor Interactive Server Components
builder.Services.AddRazorComponents()
                .AddInteractiveServerComponents();

// 5. MVC Controllers + Views (nếu bạn có cả 2)
builder.Services.AddControllersWithViews();

var app = builder.Build();

// 6. Cấu hình middleware pipeline
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseSession();        // <— PHẢI đặt trước UseRouting/UseAuthorization nếu bạn dùng session trong auth
app.UseRouting();

app.UseAuthorization();


// 7. Map endpoints

app.MapRazorComponents<App>()
   .AddInteractiveServerRenderMode();

// (Nếu bạn còn Razor Pages)
// app.MapRazorPages();

app.Run();
