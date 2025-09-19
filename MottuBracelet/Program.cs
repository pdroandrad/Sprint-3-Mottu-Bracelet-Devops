using Microsoft.EntityFrameworkCore;
using MottuBracelet.Data;
using MottuBracelet.Services;
using Oracle.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Adiciona serviços de controller
builder.Services.AddControllers();

// Configura Swagger/OpenAPI
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Configura a conexão com SQL Server
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(
        builder.Configuration.GetConnectionString("DefaultConnection"),
        sqlOptions => sqlOptions.EnableRetryOnFailure(
            maxRetryCount: 5,
            maxRetryDelay: TimeSpan.FromSeconds(10),
            errorNumbersToAdd: null
        )
    )
);


// Registra os serviços da aplicação
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
