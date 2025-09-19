using Microsoft.EntityFrameworkCore;
using MottuBracelet.Data;
using MottuBracelet.Services;

var builder = WebApplication.CreateBuilder(args);

// Lê variáveis de ambiente do .env
var server = Environment.GetEnvironmentVariable("DB_HOST");
var port = Environment.GetEnvironmentVariable("DB_PORT") ?? "1433"; // padrão SQL Server
var database = Environment.GetEnvironmentVariable("DB_NAME");
var user = Environment.GetEnvironmentVariable("DB_USER");
var password = Environment.GetEnvironmentVariable("DB_PASS");

// Monta a connection string do SQL Server
var connectionString = $"Server={server},{port};Database={database};User Id={user};Password={password};TrustServerCertificate=True;Encrypt=True;";

// Configura o DbContext com SQL Server
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(connectionString, sqlOptions =>
        sqlOptions.EnableRetryOnFailure(
            maxRetryCount: 5,
            maxRetryDelay: TimeSpan.FromSeconds(10),
            errorNumbersToAdd: null
        )
    )
);

// Adiciona serviços e controllers
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddScoped<ServicoMotos>();
builder.Services.AddScoped<ServicoPatios>();
builder.Services.AddScoped<ServicoDispositivos>();
builder.Services.AddScoped<ServicoHistoricoPatios>();

var app = builder.Build();

// Pipeline HTTP
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();
app.Run();
