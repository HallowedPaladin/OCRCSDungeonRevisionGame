using InsigniaServer.Auth;
using InsigniaServer.Contexts;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container
var jwtIssuer = builder.Configuration.GetSection("Jwt:Issuer").Get<string>();
var jwtSecretString = builder.Configuration.GetSection("Jwt:Secret").Get<string>();
var jwtSecretLength = builder.Configuration.GetSection("Jwt:SecretLength").Get<int>();
var expiryTimeInMinutes = builder.Configuration.GetSection("Jwt:ExpiryTimeInMinutes").Get<int>();

// If the secret string has not been set in the configuration then generate a random secret.
// Using a random secret is more secure and will change the secret if the server is restarted.
if (jwtSecretString != null || jwtSecretString.Length == 0)
{ 
    jwtSecretString = SecretGenerator.GenerateSecret(jwtSecretLength);
}

// Create a singleton of the TokenUtility with a new secret key and the expiry time in minutes
builder.Services.AddSingleton<TokenUtility>(new TokenUtility(jwtSecretString, expiryTimeInMinutes, jwtIssuer));

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
 .AddJwtBearer(options =>
 {
     options.TokenValidationParameters = new TokenValidationParameters
     {
         ValidateIssuer = true,
         ValidateAudience = true,
         ValidateLifetime = true,
         ValidateIssuerSigningKey = true,
         ValidIssuer = jwtIssuer,
         ValidAudience = jwtIssuer,
         IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSecretString))
     };
 });

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

// Add services to the container.
builder.Services.AddControllers();

// Configure Swagger
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Once all the services have been added then build the app.
var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();
app.Run();