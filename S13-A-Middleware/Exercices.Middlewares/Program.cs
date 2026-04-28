using Exercices.Middlewares.Data;
using Exercices.Middlewares.Extensions;
using Exercices.Middlewares.Middlewares;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews().AddRazorRuntimeCompilation();
builder.Services.AddDbContext<ApplicationDbContext>(options => options
    .UseLoggerFactory(LoggerFactory.Create(builder => { builder.AddConsole(); }))
    .UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

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

app.UseCustomError404PageMiddleware();
app.UseMiddleware<PerformanceProfiler>();

app.UseRouting();

app.UseHttpsRedirection();
app.UseRouting();

app.UseAuthorization();

app.MapStaticAssets();

app.MapDefaultControllerRoute().WithStaticAssets();

app.Run();
