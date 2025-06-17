using HealthCheckSessionTinVT.BlazorWebApp.Components;
using Microsoft.EntityFrameworkCore;
using SMMS.Repositories.TinVT;
using SMMS.Repositories.TinVT.Models;
using SMMS.Services.TinVT;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();
builder.Services.AddScoped<IServiceProviders, ServiceProviders>();
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
builder.Services.AddScoped<IHealthCheckSessionTinVTService, HealthCheckSessionTinVTService>();
builder.Services.AddScoped<UserAccountService>();
builder.Services.AddScoped<HealthCheckStudentTinVtService>();
builder.Services.AddScoped<HealthCheckStudentTinVtRepository>();
builder.Services.AddDbContext<SU25_PRN222_SE1706_G1_SMMSContext>(options =>
    options.UseSqlServer(
        builder.Configuration.GetConnectionString("DefaultConnection")
    )
);
var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseStaticFiles();
app.UseAntiforgery();

app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();

app.Run();
