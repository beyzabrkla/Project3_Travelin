using BusinessLayer.Services.CategoryServices;
using BusinessLayer.Services.CommentServices;
using BusinessLayer.Services.GuideServices;
using BusinessLayer.Services.TourServices;
using BusinessLayer.Settings;
using Microsoft.Extensions.Options;
using System.Reflection;
using AutoMapper;


var builder = WebApplication.CreateBuilder(args);

builder.Services.AddScoped<ICommentService, CommentService>(); //DI mantýðý ICommentService istenildiðinde CommentService sunuluyor (metotlarýyla)== best practices
builder.Services.AddScoped<ICategoryService, CategoryService>(); 
builder.Services.AddScoped<ITourService, TourService>();
builder.Services.AddScoped<IGuideService, GuideService>();

// GeneralMapping sýnýfýnýn olduðu assembly'yi taramasýný söylüyoruz
builder.Services.AddAutoMapper(typeof(DTOLayer.Mapping.GeneralMapping));//- Projedeki tüm Profile sýnýflarýný (AutoMapper konfigürasyonlarýný) bulup yükler.


builder.Services.Configure<DatabaseSettings>(builder.Configuration.GetSection("DatabaseSettings")); //- appsettings.json içindeki "DatabaseSettings" bölümünü alýyor. 

builder.Services.AddScoped<IDatabaseSettings>(sp =>
{
    return sp.GetRequiredService<IOptions<DatabaseSettings>>().Value;
});

// Add services to the container.
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
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
