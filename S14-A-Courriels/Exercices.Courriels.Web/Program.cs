using Exercices.Courriels.Web.Configuration;
using Exercices.Courriels.Web.Data;
using Exercices.Courriels.Web.Interfaces;
using Exercices.Courriels.Web.Services;
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

var emailConfig = builder.Configuration
        .GetRequiredSection("EmailConfiguration")
        .Get<EmailConfiguration>();
builder.Services.AddSingleton(emailConfig!);

// Configurer Resend si une clé API est fournie
if (!string.IsNullOrEmpty(emailConfig?.ApiKey))
{
    builder.Services.AddOptions<Resend.ResendClientOptions>().Configure(o =>
    {
        o.ApiToken = emailConfig.ApiKey;
    });
    builder.Services.AddHttpClient<Resend.ResendClient>();
}

builder.Services.AddScoped<IEmailSender, EmailService>();

builder.Services.AddScoped<INewsletterService, NewsletterInMemoryService>(); // Liste en mémoire au lieu d'une BD.
//builder.Services.AddScoped<INewsletterService, NewsletterService>();

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

app.MapStaticAssets();

//app.UseAntiforgery();

//app.MapControllerRoute(
//    name: "default",
//    pattern: "{controller=Home}/{action=Index}/{id?}")
app.MapDefaultControllerRoute().WithStaticAssets();

app.Run();
