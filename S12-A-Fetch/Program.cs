var builder = WebApplication.CreateBuilder(args);


// Add services to the container.
if (builder.Environment.IsDevelopment())
{
    // Ajouter le RazorRuntimeCompilation seulement lorsqu'on est en mode développement
    builder.Services.AddControllersWithViews(options =>
        options.ModelBindingMessageProvider.SetValueMustNotBeNullAccessor(_ => "Ce champ est obligatoire"))
        .AddRazorRuntimeCompilation();
}
else
{
    builder.Services.AddControllersWithViews(options =>
        options.ModelBindingMessageProvider.SetValueMustNotBeNullAccessor(_ => "Ce champ est obligatoire"));
}

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

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
