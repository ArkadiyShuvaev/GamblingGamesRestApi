using GamblingGamesRestApi.Contexts;
using GamblingGamesRestApi.Infrastructure;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Reflection;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();

addSwagger(builder);

builder.Services.AddControllers();

builder.Services.AddDbContext<ApplicationDbContext>(options => options.UseInMemoryDatabase("AppDb"));
//builder.Services.AddDbContext<ApplicationDbContext>(options => options.UseSqlServer("Server=tcp:127.0.0.1,1433;Database=AppDb;User ID=sa;Password=DevSqlS3rv3r$;Encrypt=True;TrustServerCertificate=True;Connection Timeout=30;Max Pool Size=500;"));

builder.Services.AddAuthorization();

builder.Services.AddIdentity<ApplicationUser, IdentityRole>()
    .AddEntityFrameworkStores<ApplicationDbContext>()
    .AddDefaultTokenProviders();

AddAutentication(builder);

builder.Services.AddAuthentication().AddBearerToken();

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
                Description = "To start using the API, please register a new account, then login in. " +
                "Please use the <b>Authorize</b> button to add JWT to each request you send to the API. " +
                "Please note that the authorization key should be added in the format: <b>bearer <your-token></b>"
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

static void AddAutentication(WebApplicationBuilder builder)
{
    // TODO move the key to appsettings.json
    var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("Gambling-Games-Rest-Api-Key-Super-Key"));
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