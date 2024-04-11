using ApplicationLayer.Contracts;
using InfrastructureLayer.Data;
using InfrastructureLayer.Implementations;
using Microsoft.EntityFrameworkCore;
using Microsoft.Net.Http.Headers;
using System;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddDbContext<AppDbContext>(options =>
{
    options.UseNpgsql(builder.Configuration.GetConnectionString("CleanArchitecture") ??
        throw new InvalidOperationException("Sorry, your connection is not found"));
});
builder.Services.AddScoped<IEmployee, EmployeeRepo>();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
    app.UseCors(policy =>
    {
        policy.WithOrigins("https://localhost:7012")
        .AllowAnyMethod()
        .AllowAnyHeader()
        .WithHeaders(HeaderNames.ContentType);
    });
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
