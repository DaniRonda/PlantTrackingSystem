using infrastructure;
using infrastructure.DataModels;
using service;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
if (builder.Environment.IsDevelopment())
{
    builder.Configuration.AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);
    builder.Services.AddNpgsqlDataSource("Host=abul.db.elephantsql.com;Username=tivogyll;Password=D_aMUJJ9FWKwv0clIEsj4hoJzhuCf10E;Database=tivogyll",
        dataSourceBuilder => dataSourceBuilder.EnableParameterLogging());
}
else if (builder.Environment.IsProduction())
{
    builder.Configuration.AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);
    builder.Services.AddNpgsqlDataSource("Host=abul.db.elephantsql.com;Username=tivogyll;Password=D_aMUJJ9FWKwv0clIEsj4hoJzhuCf10E;Database=tivogyll",
        dataSourceBuilder => dataSourceBuilder.EnableParameterLogging());
    
}

builder.Services.AddSingleton<DataRecordRepository>();
builder.Services.AddSingleton<DataRecordService>();
builder.Services.AddSingleton<PlantRepository>();
builder.Services.AddSingleton<PlantService>();

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
    options.WithOrigins("http://localhost:4200", "http://192.168.1.37")
        .AllowAnyMethod()
        .AllowAnyHeader()
        .AllowCredentials();
});

app.MapControllers();

app.Run();