using Exercice_Formulaire_Web.Services;
using Exercices_Formulaire_Web.Data;
using Exercices_Formulaire_Web.Services;
using Microsoft.EntityFrameworkCore;
using System.Globalization;

var builder = WebApplication.CreateBuilder(args);

var mvcBuilder = builder.Services.AddControllersWithViews();

if (builder.Environment.IsDevelopment())
{
    // Ajouter le RazorRuntimeCompilation seulement lorsqu'on est en mode développement
    mvcBuilder.AddRazorRuntimeCompilation();
}

CultureInfo ci = new("fr-CA");
CultureInfo.DefaultThreadCurrentCulture = ci;

//string cultureName = Thread.CurrentThread.CurrentCulture.Name;
//CultureInfo ci = new(CultureName);
//CultureInfo.DefaultThreadCurrentUICulture = ci;
//Thread.CurrentThread.CurrentUICulture = ci;
//Thread.CurrentThread.CurrentCulture = ci;

builder.Services.AddDbContext<ApplicationDbContext>(x =>
{
    x.UseLoggerFactory(LoggerFactory.Create(builder => { builder.AddConsole(); }));
    if (builder.Environment.IsDevelopment())
    {
        // N'affichez les données sensibles qu'en mode développement uniquement.
        x.EnableSensitiveDataLogging();
    }

    x.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
});

builder.Services.AddScoped<EmployesService>();
builder.Services.AddScoped<DepartementsService>();
builder.Services.AddScoped<PaysService>();


var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseRouting();

app.UseAuthorization();

app.MapStaticAssets();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}")
    .WithStaticAssets();


app.Run();
