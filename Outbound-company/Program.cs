using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using OfficeOpenXml;
using Outbound_company.Context;
using Outbound_company.Models;
using Outbound_company.Repository;
using Outbound_company.Repository.Interface;
using Outbound_company.Services;
using Outbound_company.Services.Interfaces;
using Outbound_company.SpeedData;


var builder = WebApplication.CreateBuilder(args);


builder.Services.AddControllersWithViews();

builder.Services.Configure<AsteriskSettings>(builder.Configuration.GetSection("Asterisk"));
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection") ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.")));

builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
    {
        options.LoginPath = "/Account/Login";
        options.LogoutPath = "/Account/Logout";
        options.AccessDeniedPath = "/Account/AccessDenied";
    });

builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("AdminOnly", policy => policy.RequireRole("Admin"));
});

builder.Services.AddHttpContextAccessor();

builder.Services.AddScoped<IApplicationDbContext, ApplicationDbContext>();
builder.Services.AddScoped<ICompaniesRepository, CompaniesRepository>();
builder.Services.AddScoped<ICompaniesService, CompaniesService>();
builder.Services.AddScoped<INumberRepository, NumberRepository>();
builder.Services.AddScoped<INumberService, NumberService>();
builder.Services.AddScoped<ICallStatisticsRepository,CallStatisticsRepository>();
builder.Services.AddScoped<ICallStatisticsService, CallStatisticsService>();
builder.Services.AddScoped<IBlackListNumberRepository, BlackListNumberRepository>();
builder.Services.AddScoped<IBlackListNumberService, BlackListNumberService>();
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IUserService, UserService>();

builder.Services.AddSingleton<IAsteriskStatusService, AsteriskStatusService>();
builder.Services.AddSingleton<IAsteriskCountOfCallsService, AsteriskCountOfCallsService>();
builder.Services.AddSingleton<ICallsManagementService, CallsManagementService>();

builder.Services.AddHostedService<AsteriskStatusBackgroundService>();


ExcelPackage.LicenseContext = LicenseContext.NonCommercial;

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
    dbContext.Database.EnsureCreated();
    SpeedData.Initialize(dbContext);
}

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Companies}/{action=Index}/{id?}");

app.MapControllerRoute(
    name: "numberPools",
    pattern: "{controller=NumberPools}/{action=Index}/{id?}");

app.Run();
