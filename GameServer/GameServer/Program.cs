using GameServer.Contexts;
using Microsoft.EntityFrameworkCore;
using System.Text;
using Microsoft.AspNetCore.Mvc;
using GameServer.Auth;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Configuration;
using Microsoft.OpenApi.Models;
using Microsoft.Extensions.DependencyInjection;


#if ApiConventions
[assembly: ApiConventionType(typeof(DefaultApiConventions))]
#endif

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();

// Added SwaggerGen with support for Authorisation
builder.Services.AddSwaggerGen(
    options =>
    {
        options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
        {
            In = ParameterLocation.Header,
            Description = "Enter a valid JSON web token here",
            Name = "Authorization",
            Type = SecuritySchemeType.Http,
            BearerFormat = "JWT",
            Scheme = "Bearer"
        });
        options.AddSecurityRequirement(new OpenApiSecurityRequirement
        {
            {
                new OpenApiSecurityScheme
                {
                    Reference = new OpenApiReference
                    {
                        Type=ReferenceType.SecurityScheme,
                        Id="Bearer"
                    }
                },
                Array.Empty<string>()
            }
        });
    });


// Database
builder.Services.AddDbContext<InsigniaDBContext>(options =>
    {
        options.UseMySql(builder.Configuration.GetConnectionString("DefaultConnection"),
                   new MySqlServerVersion(new Version(8, 0, 36))); // Specify your MySQL server version here
    });

// JWT Tokens


var jwtIssuer = builder.Configuration.GetSection("Jwt:Issuer").Get<string>();
var secretGenerator = new SecretGenerator();
var jwtSecret = SecretGenerator.GenerateSecret(builder.Configuration.GetSection("Jwt:SecretLength").Get<int>());

// Create a singleton of the TokenUtility with a new secret key and the expiry time in minutes
builder.Services.AddSingleton<TokenUtility>(new TokenUtility(jwtSecret, builder.Configuration.GetSection("Jwt:ExpiryTimeInMinutes").Get<int>()));

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
 .AddJwtBearer(options =>
 {
     var key = Convert.FromBase64String(jwtSecret);
     options.TokenValidationParameters = new TokenValidationParameters
     {
         ValidateIssuerSigningKey = true,
         IssuerSigningKey = new SymmetricSecurityKey(key),
         ValidateIssuer = false,
         ValidateAudience = false,
         //ValidateLifetime = true, 
         //ValidIssuer = jwtIssuer,
         //ValidAudience = jwtIssuer,
         //IssuerSigningKey = new SymmetricSecurityKey(Convert.FromBase64String(jwtSecret))
     };
 });

// Once all the services have been added then build the app.
var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

// For JWT
app.UseAuthentication();

app.MapControllers();

app.Run();

