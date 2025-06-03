using System.Reflection;
using Identity.WebApp.Data;
using Identity.WebApp.Factories;
using Identity.WebApp.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllersWithViews();

builder.Services
    .AddIdentity<User, IdentityRole>(options =>
    {
        options.SignIn.RequireConfirmedEmail = true;
        options.Password.RequireNonAlphanumeric = true;
        options.Password.RequireLowercase = true;
        options.Password.RequireUppercase = true;
        options.Password.RequiredLength = 8;

        options.Lockout.MaxFailedAccessAttempts = 3;
        options.Lockout.AllowedForNewUsers = true;
    })
    .AddEntityFrameworkStores<UserDbContext>()
    .AddDefaultTokenProviders()
    .AddPasswordValidator<PasswordValidator<User>>();

builder.Services.ConfigureApplicationCookie(options => options.LoginPath = "/Home/Login");

builder.Services.AddScoped<IUserClaimsPrincipalFactory<User>, UserClaimsPrincipalFactory>();

builder.Services.Configure<DataProtectionTokenProviderOptions>(options => options.TokenLifespan = TimeSpan.FromHours(3));

var migrationAssembly = typeof(Program).GetTypeInfo().Assembly.GetName().Name;
builder.Services
    .AddDbContext<UserDbContext>(options => options.UseSqlServer(builder.Configuration.GetConnectionString("IdentityWebApp"), sql => sql.MigrationsAssembly(migrationAssembly)));

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseAuthentication();

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
