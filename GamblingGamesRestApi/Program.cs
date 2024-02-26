using GamblingGamesRestApi.Contexts;
using GamblingGamesRestApi.Infrastructure;
using GamblingGamesRestApi.Repositories;
using GamblingGamesRestApi.Services;
using GamblingGamesRestApi.Settings;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Reflection;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

var environment = builder.Environment;
builder.Configuration
    .AddJsonFile("appsettings.json", false)
    .AddJsonFile($"appsettings.{environment}.json", true)
    .AddEnvironmentVariables();

var settings = builder.Configuration.Get<ServiceSettings>();
builder.Services.AddSingleton<ServiceSettings>(settings);

builder.Services.AddEndpointsApiExplorer();

addSwagger(builder);

builder.Services.AddControllers();

builder.Services.AddDbContext<ApplicationIdentityContext>(options => options.UseInMemoryDatabase("AppDb"));
builder.Services.AddDbContext<ApplicationDbContext>(options => options.UseInMemoryDatabase("AppDb"));

builder.Services.AddAuthorization();

builder.Services.AddIdentity<ApplicationUser, IdentityRole>()
    .AddEntityFrameworkStores<ApplicationIdentityContext>()
    .AddDefaultTokenProviders();

AddAutentication(builder, settings);

builder.Services.AddAuthentication().AddBearerToken();

builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IBetService, BetService>();
builder.Services.AddScoped<IPointRepository, PointRepository>();
builder.Services.AddScoped<IUserManagerWrapper, UserManagerWrapper>();

var app = builder.Build();
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.MapControllers();
app.UseAuthentication();
app.UseAuthorization();

using var scope = app.Services.CreateScope();
var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
context.Database.EnsureCreated();

app.Run();

static void addSwagger(WebApplicationBuilder builder)
{
    builder.Services.AddSwaggerGen(options =>
    {
        options.SwaggerDoc(
            "v1",
            new OpenApiInfo
            {
                Title = "Gambling Games Api",
                Version = "v1",
                Description = @"
    To start using the API:
    1. Register a new account.
    2. Login in. 
    3. Use the ""Authorize"" button to add JWT to each request you send to the API. Please note that the authorization key should be added in the format: bearer <your-token>.

    The API generates the error response in the following format:
    
    {
        ""status"": 400,
        ""traceId"": ""|2f3e3d4e-4c7e4b4e4c7e4b4e."",
        ""errors"": {
            ""Error Code 1"": [
                ""Error description.""
            ],
            ""Error Code 2"": [
                ""Error description.""
            ]
        }
    }
"
            });
        var xmlFilename = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
        options.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, xmlFilename), includeControllerXmlComments: true);

        options.UseAllOfToExtendReferenceSchemas();

        options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
        {
            Name = "Authorization",
            Type = SecuritySchemeType.ApiKey,
            Scheme = "Bearer",
            BearerFormat = "JWT",
            In = ParameterLocation.Header,
            Description = "Enter JWT Token with bearer in the format: <b>bearer <your-token></b> format. E.g.: bearer eyJhbGci....5jl-0",
        });

        options.AddSecurityRequirement(new OpenApiSecurityRequirement {
        {
            new OpenApiSecurityScheme
            {
            Reference = new OpenApiReference
            {
                Type = ReferenceType.SecurityScheme,
                Id = "Bearer"
            }
            },
            Array.Empty<string>()
        }});
    });
}

static void AddAutentication(WebApplicationBuilder builder, ServiceSettings settings)
{
    var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(settings.JwtKey));
    builder.Services.AddAuthentication(options =>
    {
        options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
        options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    })
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = "GamblingGamesCompany",
            ValidAudience = "GamblingGamesCompany",
            IssuerSigningKey = key,
            ClockSkew = TimeSpan.FromSeconds(5)
        };
    });
}