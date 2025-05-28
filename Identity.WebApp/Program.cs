using Identity.WebApp.Models;
using Identity.WebApp.Stores;
using Microsoft.AspNetCore.Identity;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllersWithViews();

builder.Services.AddIdentityCore<User>(options => { });
builder.Services.AddScoped<IUserStore<User>, UserStore>();
builder.Services
    .AddAuthentication("cookies")
    .AddCookie("cookies", options => options.LoginPath = "/Home/Login");

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
