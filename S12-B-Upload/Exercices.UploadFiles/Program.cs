using Exercices.UploadFiles.Data;
using Exercices.UploadFiles.Services;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

//// Configurer Kestrel pour accepter des fichiers plus volumineux
//builder.Services.Configure<KestrelServerOptions>(options =>
//{
//    options.Limits.MaxRequestBodySize = 104857600; // 100 MB
//});

//// Configurer les limites de formulaire
//builder.Services.Configure<FormOptions>(options =>
//{
//    options.MultipartBodyLengthLimit = 104857600; // 100 MB
//    options.ValueLengthLimit = int.MaxValue;
//    options.MultipartHeadersLengthLimit = int.MaxValue;
//});

var mvcBuilder = builder.Services.AddControllersWithViews();

if (builder.Environment.IsDevelopment())
{
    // Ajouter le RazorRuntimeCompilation seulement lorsqu'on est en mode développement
    mvcBuilder.AddRazorRuntimeCompilation();
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
// Injection de dépendances des services
builder.Services.AddScoped<IImageService, ImageService>();
builder.Services.AddScoped<IFileStorageService, FileStorageService>();

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
