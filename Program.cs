using Microsoft.AspNetCore.Mvc;
using OfficeOpenXml;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddCors(); // Make sure you call this previous to AddMvc
ExcelPackage.LicenseContext = LicenseContext.NonCommercial; // Set the license context

builder.Services.AddControllers();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors(
        options => options.WithOrigins("*").AllowAnyMethod().AllowAnyHeader()
    );

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
