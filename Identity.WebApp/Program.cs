using System.Reflection;
using Identity.WebApp.Data;
using Identity.WebApp.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllersWithViews();

builder.Services.AddIdentityCore<User>(options => { });
builder.Services.AddScoped<IUserStore<User>, UserOnlyStore<User, UserDbContext>>();
builder.Services
    .AddAuthentication("cookies")
    .AddCookie("cookies", options => options.LoginPath = "/Home/Login");

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
