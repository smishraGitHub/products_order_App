using Microsoft.EntityFrameworkCore;
using MyConsoleApp.Data;
using MyConsoleApp.Dtos;
using MyConsoleApp.Models;
using MyConsoleApp.Endpoints;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

app.MapProductEndpoints();
app.MapOrderEndpoints();

app.UseSwagger();
app.UseSwaggerUI();

app.Run();
