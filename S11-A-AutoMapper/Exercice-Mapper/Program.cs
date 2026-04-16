using Exercice_Mapper.Data;
using Exercice_Mapper.Services;
using Mapster;
using Microsoft.EntityFrameworkCore;
using System.Globalization;
using System.Reflection;

var ci = new CultureInfo("fr-CA");
//CultureInfo.DefaultThreadCurrentCulture = ci;
CultureInfo.CurrentCulture = ci;

var builder = WebApplication.CreateBuilder(args);
// Add services to the container.

var mvcBuilder = builder.Services.AddControllersWithViews(options =>
{
    options.ModelBindingMessageProvider.SetValueMustNotBeNullAccessor(_ => "Ce champ est obligatoire");
    options.ModelBindingMessageProvider.SetValueIsInvalidAccessor(_ => "Ce champ est obligatoire");
});

if (builder.Environment.IsDevelopment())
{
    mvcBuilder.AddRazorRuntimeCompilation();
}

builder.Services.AddDbContext<ApplicationDbContext>(options =>
{
    if (builder.Environment.IsDevelopment())
    {
        // N'affichez les données sensibles qu'en mode développement uniquement.
        options.EnableSensitiveDataLogging();
    }
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
});

// Services
builder.Services.AddScoped<EmployesService>();
builder.Services.AddScoped<PaysService>();
builder.Services.AddScoped<DepartementsService>();

// MAPSTER
builder.Services.AddMapster();

var app = builder.Build();

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

app.UseAuthorization();

//app.UseAntiforgery();

app.MapStaticAssets();

TypeAdapterConfig.GlobalSettings.Scan(Assembly.GetExecutingAssembly());

app.MapDefaultControllerRoute().WithStaticAssets();

app.Run();
