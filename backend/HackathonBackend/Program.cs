using Microsoft.EntityFrameworkCore;
using HackathonBackend.Data;
using HackathonBackend.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Scalar.AspNetCore;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// =========================
// SERVICES
// =========================

builder.Services.AddControllers();

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(
        builder.Configuration.GetConnectionString("DefaultConnection")
    )
);

// Email service (file-based "outbox" for hackathon demo)
builder.Services.AddScoped<IEmailService, EmailService>();

// =========================
// JWT AUTHENTICATION
// =========================

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = false,
            ValidateAudience = false,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,

            IssuerSigningKey = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(
                    "THIS_IS_MY_SUPER_SECRET_JWT_KEY_123456789"
                )
            )
        };
    });

builder.Services.AddAuthorization();

// =========================
// CORS
// =========================

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAngular",
        policy =>
        {
            policy.AllowAnyOrigin()
                  .AllowAnyHeader()
                  .AllowAnyMethod();
        });
});

// =========================
// OpenAPI (NET 10 native)
// =========================

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddOpenApi();

var app = builder.Build();

// =========================
// CREATE PRESCRIPTIONS / EMAIL FOLDERS
// =========================

var prescriptionsPath = Path.Combine(
    app.Environment.ContentRootPath,
    "wwwroot",
    "prescriptions"
);

if (!Directory.Exists(prescriptionsPath))
{
    Directory.CreateDirectory(prescriptionsPath);
}

var emailsPath = Path.Combine(
    app.Environment.ContentRootPath,
    "wwwroot",
    "emails"
);

if (!Directory.Exists(emailsPath))
{
    Directory.CreateDirectory(emailsPath);
}

// =========================
// PIPELINE
// =========================

if (app.Environment.IsDevelopment())
{
    // OpenAPI JSON document (native NET 10)
    app.MapOpenApi();

    // Modern OpenAPI UI at /swagger that reads /openapi/v1.json
    // Scalar handles the JWT "Authorize" button via its own preferred-security
    // config (no Microsoft.OpenApi.Models needed).
    app.MapScalarApiReference("/swagger", options =>
    {
        options.WithTitle("ByteBrigade Pharmacy API");
        options.WithOpenApiRoutePattern("/openapi/v1.json");
        options.WithPreferredScheme("Bearer");
        options.WithHttpBearerAuthentication(bearer =>
        {
            bearer.Token = "paste-your-jwt-token-here";
        });
    });
}

app.UseHttpsRedirection();

app.UseStaticFiles();

app.UseCors("AllowAngular");

app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

app.Run();
