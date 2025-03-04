using Comanda.Api;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using SistemaDeComandas.BancoDeDados;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

//obtem o endereco do banco de dados
var conexao = builder.Configuration.GetConnectionString("Conexao");

builder.Services.AddDbContext<ComandaContexto>(config =>
{
    config.UseMySql(conexao, ServerVersion.Parse("10.4.28-MariaDB"));
});

var chaveSecretaHex = "1ec2d3ace73de4d656f76a1727fa957757fdae32b9a22176480a0c8d52149ffb";

var chaveSecretaBytes = new byte[chaveSecretaHex.Length / 2];
for (int i = 0; i < chaveSecretaBytes.Length; i++)
{
    chaveSecretaBytes[i] = Convert.ToByte(chaveSecretaHex.Substring(i * 2, 2), 16);
}

var chaveSecreta = new SymmetricSecurityKey(chaveSecretaBytes);
var credenciais = new SigningCredentials(chaveSecreta, SecurityAlgorithms.HmacSha256);

// Configura��o da autentica��o JWT
builder.Services.AddAuthentication(opt =>
{
    opt.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    opt.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
      .AddJwtBearer(options =>
      {
          options.TokenValidationParameters = new TokenValidationParameters
          {
              ValidateIssuer = false,
              ValidateAudience = false,
              ValidateLifetime = true,
              ValidateIssuerSigningKey = false,

              IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("3e8acfc238f45a314fd4b2bde272678ad30bd1774743a11dbc5c53ac71ca494b"))
          };
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
// adicionado swagger personalizado
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "API Comandas", Version = "v1" });
    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Description =
            "JWT Authorization Header - utilizado com Bearer Authentication.\r\n\r\n" +
            "Digite 'Bearer' [espa�o] e ent�o seu token no campo abaixo.\r\n\r\n" +
            "Exemplo (informar sem as aspas): 'Bearer 12345abcdef'",
        Name = "Authorization",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.ApiKey,
        Scheme = "Bearer",
        BearerFormat = "JWT",
    });
    c.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            Array.Empty<string>()
        }
    });
});

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



app.UseCors("AllowAllOrigins"); // Aplica a pol�tica CORS

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
