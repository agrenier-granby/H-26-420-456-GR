using Exercices.BackgroundTasks.Web.BackgroundServices;
using Exercices.BackgroundTasks.Web.Data;
using Exercices.BackgroundTasks.Web.Services;
using Microsoft.EntityFrameworkCore;
using System.Globalization;

CultureInfo.CurrentCulture = new("fr-CA");

var builder = WebApplication.CreateBuilder(args);

var mvcBuilder = builder.Services.AddControllersWithViews();

if (builder.Environment.IsDevelopment())
{
    // Ajouter le RazorRuntimeCompilation seulement lorsqu'on est en mode développement
    mvcBuilder.AddRazorRuntimeCompilation();

    builder.Services.AddDatabaseDeveloperPageExceptionFilter();
}

// Add services to the container.
builder.Services.AddDbContext<ApplicationDbContext>(x =>
{
    if (builder.Environment.IsDevelopment())
    {
        // N'affichez les données sensibles qu'en mode développement uniquement.
        x.EnableSensitiveDataLogging();
    }
    x.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));

});

builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddHostedService<BirthdayBackgroundService>();

var app = builder.Build();

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
    pattern: "{controller=Users}/{action=Index}/{id?}");

app.Run();
