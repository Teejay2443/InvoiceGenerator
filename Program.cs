using AspNetCoreHero.ToastNotification;
using InvoiceGenerator.Interface.IRepositories;
using InvoiceGenerator.Interface.IServices;
using InvoiceGenerator.Models;
using InvoiceGenerator.Repository;
using InvoiceGenerator.Services;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

// Configure the ApplicationDbContext with a connection string
var dbConnection = builder.Configuration.GetConnectionString("DataBaseConnection");
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(dbConnection));

// Toast Notification
builder.Services.AddNotyf(config =>
{
    config.DurationInSeconds = 5;
    config.IsDismissable = true;
    config.Position = NotyfPosition.TopRight;
});

// Register custom services and repositories
builder.Services.AddScoped<IInvoiceServices, InvoiceServices>();
builder.Services.AddScoped<IInvoiceRepository, InvoiceRepository>();
builder.Services.AddScoped<IServiceRenderServices, ServiceRenderServices>();
builder.Services.AddScoped<IServiceRenderRepository, ServiceRenderRepository>();
builder.Services.AddScoped<IAreaCoveredServices, AreaCoveredServices>();
builder.Services.AddScoped<IAreaCoveredRepository, AreaCoveredRepository>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
