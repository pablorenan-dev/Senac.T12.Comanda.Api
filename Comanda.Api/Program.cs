using Comanda.Api;
using Microsoft.EntityFrameworkCore;
using SistemaDeComandas.BancoDeDados;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

//obtem o endereco do banco de dados
var conexao = builder.Configuration.GetConnectionString("Conexao");

builder.Services.AddDbContext<ComandaContexto>(config =>
{
    config.UseMySql(conexao, ServerVersion.Parse("10.4.28-MariaDB"));
});

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAllOrigins",
        builder =>
        {
            builder.AllowAnyOrigin()
                   .AllowAnyMethod()
                   .AllowAnyHeader();
        });
});

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// AQUI A CRIACAO DO BANCO
// essas linhas de codigo vao criar o banco, ele vai fazer o migrate para mim automaticamente
// nao preciso mais usar Add-Migraion, UpdateDatabase...
using(var e = app.Services.CreateScope())
{
    // obtendo todas as tabelas do banco
    var banco = e.ServiceProvider.GetRequiredService<ComandaContexto>();

    banco.Database.Migrate();
    // Semear os dados inicias
    InicializarDados.Semear(banco);
}



app.UseCors("AllowAllOrigins"); // Aplica a política CORS

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
