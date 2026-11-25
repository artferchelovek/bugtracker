using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Scalar.AspNetCore;
using server.Data;
using server.Models;
using server.Repositories;
using server.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(policy =>
    {
        policy.AllowAnyOrigin()
              .AllowAnyHeader()
              .AllowAnyMethod();
    });
});

builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.PropertyNamingPolicy = null;
    });

var secretKey = builder.Configuration["Jwt:SecretKey"]
    ?? builder.Configuration["Jwt:Key"]
    ?? throw new InvalidOperationException("JWT SecretKey is not configured.");

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.MapInboundClaims = false;
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = false,
            ValidateAudience = false,
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey)),
            ValidateLifetime = true,
            ClockSkew = TimeSpan.Zero,
            RoleClaimType = "role",
            NameClaimType = "sub"
        };
    });

builder.Services.AddAuthorization();

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddScoped<IAdminRepository, AdminRepository>();
builder.Services.AddScoped<IAdminService, AdminService>();
builder.Services.AddScoped<IAuthRepository, AuthRepository>();
builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddScoped<ISoftwareRepository, SoftwareRepository>();
builder.Services.AddScoped<ISoftwareService, SoftwareService>();

builder.Services.AddOpenApi();

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    try
    {
        db.Database.Migrate();

        var admin = db.Administrators.FirstOrDefault();
        if (admin == null)
        {
            var adminLogin = app.Configuration["AdminSeed:Login"]
                ?? throw new InvalidOperationException("AdminSeed:Login is not configured.");
            var adminEmail = app.Configuration["AdminSeed:Email"]
                ?? throw new InvalidOperationException("AdminSeed:Email is not configured.");
            var adminPassword = app.Configuration["AdminSeed:Password"]
                ?? throw new InvalidOperationException("AdminSeed:Password is not configured.");

            admin = new Administrator
            {
                Id = Guid.NewGuid(),
                Login = adminLogin,
                PasswordHash = BCrypt.Net.BCrypt.HashPassword(adminPassword),
                Email = adminEmail
            };
            db.Administrators.Add(admin);
            db.SaveChanges();
        }
    }
    catch (Exception ex)
    {
        var logger = scope.ServiceProvider.GetRequiredService<ILogger<Program>>();
        logger.LogError(ex, "Database initialization or seeding failed.");
        throw;
    }
}

if (app.Environment.IsDevelopment() || app.Configuration.GetValue<bool>("ApiDocs:Enabled"))
{
    app.MapOpenApi("/api/openapi/{documentName}.json");
    app.MapScalarApiReference("/api/scalar", options =>
        options.WithOpenApiRoutePattern("/api/openapi/{documentName}.json"));
}

app.UseHttpsRedirection();

app.UseCors();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
