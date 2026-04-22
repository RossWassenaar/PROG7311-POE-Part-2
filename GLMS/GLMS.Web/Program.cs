/*
 * Pro C# 10 with .NET 6 Foundational Principles and Practices in Programming
 * Eleventh Edition
 * Andrew Troelsen, Philip Japikse
 * Apress, 2022
 */

using GLMS.Web.Data;
using GLMS.Web.Patterns.Factory;
using GLMS.Web.Patterns.Observer;
using GLMS.Web.Patterns.Strategy;
using GLMS.Web.Repositories;
using GLMS.Web.Services;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllersWithViews();

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

//Services
builder.Services.AddHttpClient<ICurrencyService, CurrencyService>();
builder.Services.AddScoped<IFileService, FileService>();

//Repository pattern
builder.Services.AddScoped<IClientRepository, ClientRepository>();
builder.Services.AddScoped<IContractRepository, ContractRepository>();
builder.Services.AddScoped<IServiceRequestRepository, ServiceRequestRepository>();

//Factory pattern
builder.Services.AddScoped<IContractFactory, StandardContractFactory>();
builder.Services.AddScoped<IContractFactory, PremiumContractFactory>();
builder.Services.AddScoped<IContractFactory, EnterpriseContractFactory>();
builder.Services.AddScoped<IContractFactoryProvider, ContractFactoryProvider>();

//Strategy pattern
builder.Services.AddScoped<IServiceRequestValidationStrategy, ActiveContractStrategy>();
builder.Services.AddScoped<IServiceRequestValidationStrategy, DraftContractStrategy>();
builder.Services.AddScoped<IServiceRequestValidationStrategy, ExpiredContractStrategy>();
builder.Services.AddScoped<IServiceRequestValidationStrategy, OnHoldContractStrategy>();
builder.Services.AddScoped<IValidationStrategySelector, ValidationStrategySelector>();

//Observer pattern
builder.Services.AddScoped<IContractStatusObserver, AuditLogObserver>();
builder.Services.AddScoped<IContractStatusObserver, ExpiryAlertObserver>();
builder.Services.AddScoped<IContractStatusPublisher, ContractStatusPublisher>();

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();

var defaultCulture = new System.Globalization.CultureInfo("en-US");
var localizationOptions = new RequestLocalizationOptions
{
    DefaultRequestCulture = new Microsoft.AspNetCore.Localization.RequestCulture(defaultCulture),
    SupportedCultures = new List<System.Globalization.CultureInfo> { defaultCulture },
    SupportedUICultures = new List<System.Globalization.CultureInfo> { defaultCulture }
};
app.UseRequestLocalization(localizationOptions);

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    db.Database.Migrate();
}

app.Run();

public partial class Program { }
