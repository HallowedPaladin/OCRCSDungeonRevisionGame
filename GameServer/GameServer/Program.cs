using GameServer.Contexts;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Text;


//#if ApiConventions
//[assembly: ApiConventionType(typeof(DefaultApiConventions))]
//#endif

var builder = WebApplication.CreateBuilder(args);


// JWT Tokens
// Get parameters from the app config
var jwtIssuer = builder.Configuration.GetSection("Jwt:Issuer").Get<string>();
var jwtSecretString = builder.Configuration.GetSection("Jwt:Secret").Get<string>();
//var jwtSecretLength = builder.Configuration.GetSection("Jwt:SecretLength").Get<int>();
//var expiryTimeInMinutes = builder.Configuration.GetSection("Jwt:ExpiryTimeInMinutes").Get<int>();

//var jwtSecretString = SecretGenerator.GenerateSecret(jwtSecretLength);
// var secretGenerator = new SecretGenerator();
// Create a singleton of the TokenUtility with a new secret key and the expiry time in minutes
//builder.Services.AddSingleton<TokenUtility>(new TokenUtility(jwtSecretString, expiryTimeInMinutes, jwtIssuer));

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

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
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
app.UseAuthorization();
app.UseAuthentication();
app.MapControllers();
app.Run();