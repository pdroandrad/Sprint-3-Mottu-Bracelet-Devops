using Microsoft.EntityFrameworkCore;
using MottuBracelet.Data;
using MottuBracelet.Services;
using Oracle.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Adiciona servi�os de controller
builder.Services.AddControllers();

// Configura Swagger/OpenAPI
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Configura a conex�o com Oracle
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseOracle(builder.Configuration.GetConnectionString("DefaultConnection")));

// Registra os servi�os da aplica��o
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

// Mapeia os controllers
app.MapControllers();

app.Run();
