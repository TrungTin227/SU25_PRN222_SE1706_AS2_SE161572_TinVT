using HealthCheckSession2TinVT.BlazorWebApp;
using HealthCheckSession2TinVT.BlazorWebApp.Components;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.Server.ProtectedBrowserStorage;
using SMMS.Services.TinVT;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();
builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
    {
        options.Cookie.Name = "YourAppAuthCookie";
        options.LoginPath = "/login"; // Trang sẽ chuyển hướng đến nếu chưa đăng nhập
        options.LogoutPath = "/logout";
        options.ExpireTimeSpan = TimeSpan.FromMinutes(30);
        options.SlidingExpiration = true;
    });
// *** THÊM AUTHENTICATION CONFIGURATION ***
builder.Services.AddAuthentication("CustomScheme")
    .AddScheme<CustomAuthenticationSchemeOptions, CustomAuthenticationHandler>(
        "CustomScheme", options => { });

builder.Services.AddAuthorization();

// *** THÊM CUSTOM AUTH STATE PROVIDER ***
builder.Services.AddScoped<AuthenticationStateProvider, CustomAuthStateProvider>();
builder.Services.AddCascadingAuthenticationState();

builder.Services.AddScoped<IServiceProviders, ServiceProviders>();
builder.Services.AddScoped<ProtectedSessionStorage>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    app.UseHsts();
}
// *** THÊM AUTHENTICATION MIDDLEWARE ***
app.UseAuthentication();
app.UseAuthorization();

app.UseHttpsRedirection();
app.UseStaticFiles();


app.UseAntiforgery();

app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();

app.Run();