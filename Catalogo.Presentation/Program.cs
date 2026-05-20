using CatalogoApp.Application.Services;
using CatalogoApp.Infrastructure.Repositories;
using CatalogoApp.Domain.Models;
using CatalogoApp.Domain.Interfaces;


var builder = WebApplication.CreateBuilder(args);

// Ruta del archivo JSON — se guarda en la carpeta "data" del proyecto

var jsonPath = Path.Combine(builder.Environment.ContentRootPath, "Data", "Item.json");

// Registrar el repositorio JSON como implementación ItemRepository
builder.Services.AddSingleton<IItemRepository>(
     new JsonItemRepository(jsonPath)
);

// Registrar el servicio de Application
builder.Services.AddScoped<ItemService>();

// Registrar autorización
builder.Services.AddAuthorization();

// Registrar servicios MVC
builder.Services.AddControllersWithViews();

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