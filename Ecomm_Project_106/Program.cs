using Ecomm_Project_106.DataAccess.Data;
using Ecomm_Project_106.DataAccess.Repository;
using Ecomm_Project_106.DataAccess.Repository.IRepository;
using Ecomm_Project_106.Utility;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.CodeAnalysis.Options;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Stripe;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
var cs = builder.Configuration.GetConnectionString("Core") ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(cs));
builder.Services.AddDatabaseDeveloperPageExceptionFilter();

//builder.Services.AddDefaultIdentity<IdentityUser>(options => options.SignIn.RequireConfirmedAccount = true)
//    .AddEntityFrameworkStores<ApplicationDbContext>();
builder.Services.AddIdentity <IdentityUser,IdentityRole>().AddDefaultTokenProviders().AddEntityFrameworkStores<ApplicationDbContext>();
    
builder.Services.AddControllersWithViews().AddRazorRuntimeCompilation();
builder.Services.AddRazorPages();
//builder.Services.AddScoped<ICategoryRepository,CategoryRepository>();
//builder.Services.AddScoped<ICoverTypeRepository, CoverTypeRepository>();
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
builder.Services.AddScoped<IEmailSender, EmailSender>();
builder.Services.ConfigureApplicationCookie(Options =>
{
    Options.LoginPath = $"/Identity/Account/Login";
    Options.LogoutPath = $"/Identity/Account/Login";
    Options.AccessDeniedPath = $"/Identity/Account/AccessDenied";
});
builder.Services.AddAuthentication().AddFacebook(options =>
{
    options.AppId = "746881001080423";
    options.AppSecret = "87fbb9d4a456669b4de69ce9a6389685";

});
builder.Services.AddAuthentication().AddGoogle(options =>
{
    options.ClientId = "991196153100-ttc0rst57puul428gkqrueamcq95mjdn.apps.googleusercontent.com";
    options.ClientSecret = "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";

});
builder.Services.AddAuthentication()
    .AddTwitter(options =>
    {
        options.ConsumerKey = "paQHSDiHAClVfp6aa8liDXp8d";
        options.ConsumerSecret = "TXZd0RIP3WIrxywnOtW4PDmizaUtXGJaIn97T4iqLukPms6L1F";
    });
builder.Services.AddAuthentication().AddGitHub(options =>
{
    options.ClientId = "Ov23liXghhl7o0I7yXz3";
    options.ClientSecret = "535dde1c953550098fb86698e307fb679f997b5f";

});
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(30);
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
});
builder.Services.Configure<StripeSetting>
    (builder.Configuration.GetSection("StripeSetting"));
builder.Services.Configure<EmailSettings>
    (builder.Configuration.GetSection("EmailSettings"));

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
app.UseSession();
app.UseRouting();
StripeConfiguration.ApiKey = builder.Configuration.GetSection("StripeSetting")["Secretkey"];

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{Area=Customer}/{controller=Home}/{action=Index}/{id?}");
app.MapRazorPages();

app.Run();
