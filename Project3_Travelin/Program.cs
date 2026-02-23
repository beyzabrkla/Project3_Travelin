using BusinessLayer.Abstract;
using BusinessLayer.Concrete;
using DataAccessLayer.Abstract;
using DataAccessLayer.Concrete;
using EntityLayer.Settings;
using Microsoft.Extensions.Options;

var builder = WebApplication.CreateBuilder(args);

// DataAccessLayer (Dal) Kayýtlarý
builder.Services.AddScoped<ICommentDal, MongoCommentDal>(); //DI mantýðý ICommentDal istenildiðinde MongoCommentDal sunuluyor (metotlarýyla)== best practices
builder.Services.AddScoped<ICategoryDal, MongoCategoryDal>(); 
builder.Services.AddScoped<ITourDal, MongoTourDal>();
builder.Services.AddScoped<IGuideDal, MongoGuideDal>();

// GeneralMapping sýnýfýnýn olduðu assembly'yi taramasýný söylüyoruz
builder.Services.AddAutoMapper(typeof(DTOLayer.Mapping.GeneralMapping));//- Projedeki tüm Profile sýnýflarýný (AutoMapper konfigürasyonlarýný) bulup yükler.


builder.Services.Configure<DatabaseSettings>(builder.Configuration.GetSection("DatabaseSettings")); //- appsettings.json içindeki "DatabaseSettings" bölümünü alýyor. 

builder.Services.AddScoped<IDatabaseSettings>(sp =>
{
    return sp.GetRequiredService<IOptions<DatabaseSettings>>().Value;
});

// BusinessLayer (Service) Kayýtlarý
builder.Services.AddScoped<ICategoryService, CategoryManager>();
builder.Services.AddScoped<ITourService, TourManager>();
builder.Services.AddScoped<IGuideService, GuideManager>();
builder.Services.AddScoped<ICommentService, CommentManager>();

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
