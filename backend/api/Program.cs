using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using System.Collections.Generic;
using infrastructure;
using service;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
if (builder.Environment.IsDevelopment())
{
    builder.Services.AddNpgsqlDataSource("postgres://tivogyll:D_aMUJJ9FWKwv0clIEsj4hoJzhuCf10E@abul.db.elephantsql.com/tivogyll",
        dataSourceBuilder => dataSourceBuilder.EnableParameterLogging());
}
else if (builder.Environment.IsProduction())
{
    builder.Services.AddNpgsqlDataSource("postgres://tivogyll:D_aMUJJ9FWKwv0clIEsj4hoJzhuCf10E@abul.db.elephantsql.com/tivogyll");
}

// Configuración de las tasas de conversión
var rates = new Dictionary<string, decimal>
{
    {"USD", 1m},
    {"EUR", 0.93m},
    {"GBP", 0.76m},
    {"JPY", 130.53m},
    {"AUD", 1.31m}
};
builder.Services.AddSingleton(rates);

builder.Services.AddSingleton<ConversionHistoryRepository>();
builder.Services.AddSingleton<CurrencyConversionService>();

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// Configuración de CORS
app.UseCors(options =>
{
    options.WithOrigins("http://localhost:4200") // Especifica el origen permitido
        .AllowAnyMethod()
        .AllowAnyHeader()
        .AllowCredentials();
});

app.MapControllers();

app.Run();