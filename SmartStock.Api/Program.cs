using System.Text;
using FluentValidation;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Npgsql;
using SmartStock.Application.Common;
using SmartStock.Application.DependencyInjection;
using SmartStock.Application.Dtos;
using SmartStock.Infrastructure.Data;
using SmartStock.Infrastructure.DependencyInjection;
using SmartStock.Infrastructure.Services;
using SmartStock.Api.Middleware;

static string? ResolveConnectionString()
{
    var candidates = new[]
    {
        Environment.GetEnvironmentVariable("ConnectionStrings__DefaultConnection"),
       
        new ConfigurationBuilder().AddJsonFile("appsettings.json", optional: true).AddJsonFile("appsettings.Development.json", optional: true).Build().GetConnectionString("DefaultConnection")
    };

    foreach (var candidate in candidates)
    {
        if (string.IsNullOrWhiteSpace(candidate))
        {
            continue;
        }

        var trimmed = candidate.Trim();

        if (trimmed.StartsWith("postgres://", StringComparison.OrdinalIgnoreCase)
            || trimmed.StartsWith("postgresql://", StringComparison.OrdinalIgnoreCase))
        {
            var uri = new Uri(trimmed);
            var builder = new NpgsqlConnectionStringBuilder
            {
                Host = uri.Host,
                Port = uri.IsDefaultPort ? 5432 : uri.Port,
                Database = uri.AbsolutePath.Trim('/'),
                Username = uri.UserInfo.Contains(':') ? uri.UserInfo.Split(':', 2)[0] : uri.UserInfo,
                Password = uri.UserInfo.Contains(':') ? Uri.UnescapeDataString(uri.UserInfo.Split(':', 2)[1]) : string.Empty,
                SslMode = SslMode.Require
            };

            return builder.ConnectionString;
        }

        return trimmed;
    }

    return null;
}

var builder = WebApplication.CreateBuilder(args);

var port = Environment.GetEnvironmentVariable("PORT");
if (!string.IsNullOrWhiteSpace(port))
{
    builder.WebHost.UseUrls($"http://0.0.0.0:{port}");
}

var jwtSettings = builder.Configuration.GetSection("Jwt").Get<JwtSettings>();
if (jwtSettings == null || string.IsNullOrWhiteSpace(jwtSettings.Secret))
{
    throw new InvalidOperationException("JWT settings are not configured. Set Jwt:Secret in configuration.");
}

builder.Services.Configure<JwtSettings>(builder.Configuration.GetSection("Jwt"));

var connectionString = ResolveConnectionString();
if (string.IsNullOrWhiteSpace(connectionString))
    throw new InvalidOperationException("Database connection string is not configured. Set ConnectionStrings__DefaultConnection or DATABASE_URL in the environment.");

builder.Services.AddInfrastructureServices(connectionString);
builder.Services.AddApplicationServices();
builder.Services.AddValidatorsFromAssemblyContaining<AppException>();

builder.Services.AddControllers();
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = jwtSettings.Issuer,
            ValidAudience = jwtSettings.Audience,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings.Secret)),
            ClockSkew = TimeSpan.Zero
        };
    });

builder.Services.AddAuthorization();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        In = ParameterLocation.Header,
        Description = "Please enter JWT with Bearer into field",
        Name = "Authorization",
        Type = SecuritySchemeType.ApiKey
    });
    // include xml comments from generated doc file
    var xmlFile = System.IO.Path.ChangeExtension(System.Reflection.Assembly.GetExecutingAssembly().Location, ".xml");
    if (System.IO.File.Exists(xmlFile))
    {
        c.IncludeXmlComments(xmlFile);
    }
    c.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            new string[] { }
        }
    });
});

var app = builder.Build();

var autoMigrate = builder.Configuration.GetValue<bool>("Database:AutoMigrate", false)
    || builder.Configuration.GetValue<bool>("AUTO_MIGRATE", false);

if (autoMigrate)
{
    try
    {
        using var scope = app.Services.CreateScope();
        var dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
        dbContext.Database.Migrate();
    }
    catch (Exception ex)
    {
        throw new InvalidOperationException("Auto-migration is enabled, but the database connection string is invalid or the database is unreachable. Check ConnectionStrings__DefaultConnection or DATABASE_URL.", ex);
    }
}

app.UseMiddleware<GlobalExceptionMiddleware>();

var enableSwagger = app.Environment.IsDevelopment()
    || builder.Configuration.GetValue<bool>("Swagger:Enabled", false)
    || bool.TryParse(Environment.GetEnvironmentVariable("ENABLE_SWAGGER"), out var swaggerEnabled) && swaggerEnabled;

if (enableSwagger)
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
