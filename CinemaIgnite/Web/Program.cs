using Common;
using Core.Mapping;
using Core.Services;
using Core.Services.Contracts;
using Infrastructure;
using Infrastructure.Common;
using Infrastructure.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Web.ModelBinders;
using Web.ModelBinders.Providers;

var builder = WebApplication.CreateBuilder(args);
// Add services to the container.
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<CinemaDbContext>(options =>
    options.UseSqlServer(connectionString));
builder.Services.AddDatabaseDeveloperPageExceptionFilter();

builder.Services.AddDefaultIdentity<User>(options =>
{
    options.SignIn.RequireConfirmedAccount = false;
    options.Password.RequireNonAlphanumeric = false;
    options.Password.RequireUppercase = false;
    options.User.AllowedUserNameCharacters = options.User.AllowedUserNameCharacters + UserConstants.NameAllowedCharacters;
})
    .AddRoles<IdentityRole>()
    .AddEntityFrameworkStores<CinemaDbContext>();

//builder.Services.AddAuthentication()
//    .AddFacebook(options =>
//    {
//        options.AppId = "701770074573039";
//        options.AppSecret = "05976741c42d3c2da3318a08f1b673c7";
//    });

builder.Services.AddAutoMapper(cfg => cfg.AddProfile<GenreProfile>());
builder.Services.AddAutoMapper(cfg => cfg.AddProfile<MovieProfile>());
builder.Services.AddAutoMapper(cfg => cfg.AddProfile<ProjectionProfile>());
builder.Services.AddAutoMapper(cfg => cfg.AddProfile<TicketProfile>());
builder.Services.AddAutoMapper(cfg => cfg.AddProfile<UserProfile>());
builder.Services.AddAutoMapper(cfg => cfg.AddProfile<NotificationProfile>());

builder.Services.AddScoped<IRepository, Repository>();

builder.Services.AddScoped<IGenreService, GenreService>();
builder.Services.AddScoped<IMovieService, MovieService>();
builder.Services.AddScoped<IProjectionService, ProjectionService>();
builder.Services.AddScoped<ITicketService, TicketService>();
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<INotificationService, NotificationService>();

builder.Services.AddControllersWithViews()
    .AddMvcOptions(options =>
    {
        options.ModelBinderProviders.Insert(0, new DecimalModelBinderProvider());
        options.ModelBinderProviders.Insert(1, new DateTimeModelBinderProvider(FormattingConstant.Format));
    });

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseMigrationsEndPoint();
}
else
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "Area",
    pattern: "{area:exists}/{controller=Home}/{action=Index}/{id?}");

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");
app.MapRazorPages();

app.Run();
