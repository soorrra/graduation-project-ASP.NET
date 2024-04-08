using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Petsitter.Models;
using Petsitter.Data;
using System.Configuration;
using Petsitter.Services;
using Petsitter.Data.Services;
using Microsoft.AspNetCore.Mvc.Razor;
using Microsoft.AspNetCore.Localization;
using SendGrid.Helpers.Mail;
using Petsitter;
using Microsoft.CodeAnalysis.Options;
using System.Globalization;
using Petsitter.Hubs;

var builder = WebApplication.CreateBuilder(args);
var configuration = builder.Configuration;

// Add services to the container.

builder.Services.AddScoped<IEmailService, EmailService>();
builder.Services.Configure<EmailSettings>(builder.Configuration.GetSection("EmailSettings"));
builder.Services.AddControllersWithViews()
    .AddViewLocalization(LanguageViewLocationExpanderFormat.Suffix);
builder.Services.AddSignalR();


builder.Services.AddLocalization(options =>
{
    options.ResourcesPath = "Resources"; 
});

builder.Services.Configure<RequestLocalizationOptions>(options=>
{
    var supportedCulutes = new[]
    {
        new CultureInfo("en-US"),
        new CultureInfo("ru-RU")
    };
    options.RequestCultureProviders = new List<IRequestCultureProvider>
    {
        new QueryStringRequestCultureProvider(),
        new CookieRequestCultureProvider()
    };
    options.DefaultRequestCulture = new RequestCulture("en-US");
    options.SupportedCultures = supportedCulutes;
    });

//builder.Services.Configure<Configuration>(configuration.GetSection("EmailConfiguration"));
//builder.Services.AddScoped<IEmailService, EmailService>();
builder.Services.AddControllers();

var connectionString = builder.Configuration["ConnectionStrings:DefaultConnection"];
  
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(connectionString));

builder.Services.AddDatabaseDeveloperPageExceptionFilter();

builder.Services.AddDbContext<PetsitterContext>(options =>
    options.UseSqlServer(connectionString));

builder.Services.AddDefaultIdentity<IdentityUser>(options => options.SignIn.RequireConfirmedAccount = true)
    .AddRoles<IdentityRole>()
    .AddEntityFrameworkStores<ApplicationDbContext>();
builder.Services.AddControllersWithViews();
//builder.Services.AddTransient<IEmailService, EmailService>();
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(15);
});
builder.Services.AddRazorPages();

builder.Services.AddSignalR();

var app = builder.Build();

app.UseRequestLocalization();


app.UseSession();

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
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");
app.MapRazorPages();

app.MapRazorPages();

app.UseEndpoints(endpoints =>
{
    endpoints.MapControllerRoute(
        name: "default",
        pattern: "{controller=Home}/{action=Index}/{id?}");
    endpoints.MapRazorPages();
    endpoints.MapHub<ChatHub>("/chatHub");
});

app.Run();
