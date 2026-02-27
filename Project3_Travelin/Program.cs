using AspNetCore.Identity.MongoDbCore.Extensions;
using AspNetCore.Identity.MongoDbCore.Infrastructure;
using BusinessLayer.Abstract;
using BusinessLayer.Concrete;
using DataAccessLayer.Abstract;
using DataAccessLayer.Concrete;
using EntityLayer;
using EntityLayer.Settings;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using Project3_Travelin.Filters;

var builder = WebApplication.CreateBuilder(args);

//  DataAccessLayer (Dal) Kayıtları ---
builder.Services.AddScoped<ICommentDal, MongoCommentDal>();
builder.Services.AddScoped<ITourDal, MongoTourDal>();
builder.Services.AddScoped<IGuideDal, MongoGuideDal>();
builder.Services.AddScoped<IReservationDal, MongoReservationDal>();

//  AutoMapper & Database Settings ---
builder.Services.AddAutoMapper(typeof(DTOLayer.Mapping.GeneralMapping));
builder.Services.Configure<DatabaseSettings>(builder.Configuration.GetSection("DatabaseSettings"));

builder.Services.AddScoped<IDatabaseSettings>(sp =>
{
    return sp.GetRequiredService<IOptions<DatabaseSettings>>().Value;
});

// Identity & MongoDB Yapılandırması ---
var mongoDbIdentityConfig = new MongoDbIdentityConfiguration
{
    MongoDbSettings = new MongoDbSettings
    {
        ConnectionString = builder.Configuration["DatabaseSettings:ConnectionString"],
        DatabaseName = builder.Configuration["DatabaseSettings:DatabaseName"]
    },
    IdentityOptionsAction = options =>
    {
        options.Password.RequireDigit = false;
        options.Password.RequiredLength = 6;
        options.Password.RequireNonAlphanumeric = false;
        options.Password.RequireUppercase = false;
        options.Password.RequireLowercase = false;
        options.User.RequireUniqueEmail = true;
        options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(5);
        options.Lockout.MaxFailedAccessAttempts = 5;
    }
};

// 1. Identity Servisleri
builder.Services
    .ConfigureMongoDbIdentity<AppUser, AppRole, Guid>(mongoDbIdentityConfig)
    .AddUserManager<UserManager<AppUser>>()
    .AddSignInManager<SignInManager<AppUser>>()
    .AddRoleManager<RoleManager<AppRole>>()
    .AddDefaultTokenProviders();

builder.Services.Configure<IISServerOptions>(options => {
    options.MaxRequestBodySize = 52428800; // 50MB
});

// 2. Authentication Ayarı
builder.Services.AddAuthentication(options =>
{
    options.DefaultScheme = IdentityConstants.ApplicationScheme;
    options.DefaultAuthenticateScheme = IdentityConstants.ApplicationScheme;
    options.DefaultChallengeScheme = IdentityConstants.ApplicationScheme;
})
.AddCookie(IdentityConstants.ApplicationScheme, options =>
{
    options.LoginPath = "/Home/Index/";
    options.AccessDeniedPath = "/Home/Index/";
    options.ExpireTimeSpan = TimeSpan.FromDays(7);
    options.Cookie.HttpOnly = true;
});

// 3. Ek Cookie Ayarları
builder.Services.ConfigureApplicationCookie(options =>
{
    options.LoginPath = "/Home/Index/";
    options.AccessDeniedPath = "/Home/Index/";
    options.SlidingExpiration = true;
});

// BusinessLayer (Service) Kayıtları ---
builder.Services.AddScoped<ITourService, TourManager>();
builder.Services.AddScoped<IGuideService, GuideManager>();
builder.Services.AddScoped<ICommentService, CommentManager>();
builder.Services.AddScoped<IReservationService, ReservationManager>();

builder.Services.AddSingleton(TimeProvider.System);

// FooterPhotosFilter kaydı ve global filtre olarak ekleme
builder.Services.AddScoped<FooterPhotosFilter>();
builder.Services.AddControllersWithViews(options =>
{
    options.Filters.AddService<FooterPhotosFilter>();
});

var app = builder.Build();

// HTTP Request Pipeline ---
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

// Otomatik Rol Oluşturma (Seed Data) ---
using (var scope = app.Services.CreateScope())
{
    var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<AppRole>>();
    string[] roles = { "Admin", "Guide", "Customer" };

    foreach (var role in roles)
    {
        if (!await roleManager.RoleExistsAsync(role))
        {
            await roleManager.CreateAsync(new AppRole { Name = role });
        }
    }
}

app.Run();