using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Microsoft.AspNetCore.Identity;
using FluentValidation.AspNetCore;
using MaybeFinal.Services;
using MaybeFinal.Models;
using NLog.Extensions.Logging;
using Microsoft.Extensions.Logging;
using FluentValidation;
using MaybeFinal.Contexts;
using MaybeFinal.Validations;
using MaybeFinal;

var builder = WebApplication.CreateBuilder(args);
var logDirectory = Path.Combine(Directory.GetCurrentDirectory(), "Logs");
if (!Directory.Exists(logDirectory))
{
    Directory.CreateDirectory(logDirectory);  // Create the Logs folder if it doesn't exist
}

// Configure logging
builder.Services.AddLogging();
builder.Logging.ClearProviders();
builder.Logging.AddNLog();

// Configure JwtSettings from appsettings.json
builder.Services.Configure<JwtSettings>(builder.Configuration.GetSection("Jwt"));

// Add DbContext for SQL Server
builder.Services.AddDbContext<MaybeFinalDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// Register services
builder.Services.AddScoped<LoanService>();
builder.Services.AddScoped<IJwtService, JwtService>(); // Register IJwtService and JwtService

// Add Identity services
builder.Services.AddIdentity<User, IdentityRole>(options => options.SignIn.RequireConfirmedAccount = false)
    .AddEntityFrameworkStores<MaybeFinalDbContext>()  // Use your custom DbContext
    .AddDefaultTokenProviders();  // Adds default token providers for managing authentication


// Register PasswordHasher for User model
builder.Services.AddScoped<IPasswordHasher<User>, PasswordHasher<User>>();

// Add FluentValidation to validate models
builder.Services.AddValidatorsFromAssemblyContaining<LoanRequestDtoValidator>();

// Configure Authentication with JWT Bearer
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidIssuer = builder.Configuration["Jwt:Issuer"],
            ValidAudience = builder.Configuration["Jwt:Audience"],
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Secret"]))
        };
    });

// Add Authorization policies for roles
builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("Accountant", policy => policy.RequireRole("Accountant"));
    options.AddPolicy("User", policy => policy.RequireRole("User"));
});

// Add controllers
builder.Services.AddControllers();

// Add Swagger generation services
builder.Services.AddSwaggerGen();

// Configure logging with NLog
builder.Logging.ClearProviders();
builder.Logging.AddNLog();

// Build the app
var app = builder.Build();

// Middleware
app.UseMiddleware<GlobalExceptionHandler>();
app.UseAuthentication();
app.UseAuthorization();

// Enable Swagger for API documentation in Development environment
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "My API V1");
        c.RoutePrefix = "swagger";  // Swagger UI will be available at /swagger
    });
    app.UseDeveloperExceptionPage();
}
else
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

// Map controllers (API routes)
app.MapControllers();

// Run the application
app.Run();






