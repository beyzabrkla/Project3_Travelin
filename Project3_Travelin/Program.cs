using Microsoft.Extensions.Options;
using MongoDB.Driver.Core.Operations;
using Project3_Travelin.Services.CategoryServices;
using Project3_Travelin.Services.CommentServices;
using Project3_Travelin.Services.GuideServices;
using Project3_Travelin.Services.TourServices;
using Project3_Travelin.Settings;
using System.Reflection;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddScoped<ICommentService, CommentService>(); //DI mantýðý ICommentService istenildiðinde CommentService sunuluyor (metotlarýyla)== best practices
builder.Services.AddScoped<ICategoryService, CategoryService>(); 
builder.Services.AddScoped<ITourService, TourService>();
builder.Services.AddScoped<IGuideService, GuideService>();

builder.Services.AddAutoMapper(Assembly.GetExecutingAssembly()); //AutoMapper’ýn kullanýlabilmesi için uygulamaya ekleniyor
                                                                 //- Projedeki tüm Profile sýnýflarýný (AutoMapper konfigürasyonlarýný) bulup yükler.


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
