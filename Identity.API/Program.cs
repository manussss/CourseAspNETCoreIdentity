using System.Reflection;
using System.Text;
using Identity.Domain.Entities;
using Identity.Infra;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services
    .AddIdentity<User, Role>(options =>
    {
        options.SignIn.RequireConfirmedEmail = true;
        options.Password.RequireNonAlphanumeric = true;
        options.Password.RequireLowercase = true;
        options.Password.RequireUppercase = true;
        options.Password.RequiredLength = 8;

        options.Lockout.MaxFailedAccessAttempts = 3;
        options.Lockout.AllowedForNewUsers = true;
    })
    .AddEntityFrameworkStores<WebApiDbContext>()
    .AddRoleValidator<RoleValidator<Role>>()
    .AddRoleManager<RoleManager<Role>>()
    .AddSignInManager<SignInManager<User>>()
    .AddDefaultTokenProviders()
    .AddPasswordValidator<PasswordValidator<User>>();

var migrationAssembly = typeof(Program).GetTypeInfo().Assembly.GetName().Name;
builder.Services
    .AddDbContext<WebApiDbContext>(options => options.UseSqlServer(builder.Configuration.GetConnectionString("IdentityAPI"), sql => sql.MigrationsAssembly(migrationAssembly)));

builder.Services
    .AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new()
        {
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(builder.Configuration.GetValue<string>("Token")!)),
            ValidateIssuer = false,
            ValidateAudience = false
        };
    });

builder.Services.AddCors();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors(c => c.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader());

app.UseHttpsRedirection();

var summaries = new[]
{
    "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
};

app.MapGet("/weatherforecast", () =>
{
    var forecast =  Enumerable.Range(1, 5).Select(index =>
        new WeatherForecast
        (
            DateOnly.FromDateTime(DateTime.Now.AddDays(index)),
            Random.Shared.Next(-20, 55),
            summaries[Random.Shared.Next(summaries.Length)]
        ))
        .ToArray();
    return forecast;
})
.WithName("GetWeatherForecast")
.WithOpenApi();

app.Run();

record WeatherForecast(DateOnly Date, int TemperatureC, string? Summary)
{
    public int TemperatureF => 32 + (int)(TemperatureC / 0.5556);
}
