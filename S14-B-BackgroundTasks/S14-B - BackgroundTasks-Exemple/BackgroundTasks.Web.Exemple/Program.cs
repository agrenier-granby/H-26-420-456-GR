using BackgroundTasks.Web.Exemple.BackgroundTasks;
using BackgroundTasks.Web.Exemple.Data;
using BackgroundTasks.Web.Exemple.Middlewares;
using BackgroundTasks.Web.Exemple.Models;
using BackgroundTasks.Web.Exemple.Repositories;
using BackgroundTasks.Web.Exemple.Services;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

//builder.Services.AddFluentValidationAutoValidation(c => { c.DisableDataAnnotationsValidation = true; });

// Add services to the container.
if (builder.Environment.IsDevelopment())
{
    // Ajouter le RazorRuntimeCompilation seulement lorsqu'on est en mode développement
    builder.Services.AddControllersWithViews().AddRazorRuntimeCompilation();
}
else
{
    builder.Services.AddControllersWithViews();
}

// Add services to the container.
builder.Services.AddDbContext<ApplicationDbContext>(x =>
{
    x.UseLoggerFactory(LoggerFactory.Create(builder => { builder.AddConsole(); }));
    x.EnableSensitiveDataLogging();
    x.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
});

builder.Services.AddScoped<UserRepository>();
builder.Services.AddScoped<UsersReportingService>();
builder.Services.AddScoped<UserService<User>>();
builder.Services.AddHostedService<CreatedUsersSinceYesterdayReportTask>();

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
app.UseMiddleware<ValidationExceptionMiddleware>();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
