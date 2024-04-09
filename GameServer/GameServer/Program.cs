using GameServer.Contexts;
using Microsoft.EntityFrameworkCore;
using MySql.EntityFrameworkCore;

using Microsoft.Extensions.Configuration;
using System.Text;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
//using Pomelo.EntityFrameworkCore.MySql.Infrastructure;
using MySql.EntityFrameworkCore.Extensions;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();



builder.Services.AddEntityFrameworkMySQL().AddDbContext<InsigniaDBContext>(options =>
    {
        options.UseMySQL(builder.Configuration.GetConnectionString("DefaultConnection"));
    }
);

// Database
//builder.Services.AddDbContext<InsigniaDBContext>(options =>
//{
//    options.UseMySql(builder.Configuration.GetConnectionString("DefaultConnection"),
//               new MySqlServerVersion(new Version(8, 3, 0))); // Specify your MySQL server version here
//});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();

