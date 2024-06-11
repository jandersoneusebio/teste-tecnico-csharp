using MediatR;
using Microsoft.OpenApi.Models;
using Questao5.Infrastructure.Database;
using Questao5.Infrastructure.Database.Repository;
using Questao5.Infrastructure.Sqlite;
using System.Reflection;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();

builder.Services.AddMediatR(Assembly.GetExecutingAssembly());

// sqlite
builder.Services.AddSingleton(new DatabaseConfig { Name = builder.Configuration.GetValue<string>("DatabaseName", "Data Source=database.sqlite") });
builder.Services.AddSingleton<IDatabaseBootstrap, DatabaseBootstrap>();

builder.Services.AddSingleton<IDbConnectionFactory, DbConnectionFactory>();
builder.Services.AddSingleton<IMovimentacaoRepository, MovimentacaoRepository>();
builder.Services.AddSingleton<IContaRepository, ContaRepository>();
builder.Services.AddSingleton<IIdempotenciaRepository, IdempotenciaRepository>();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();

builder.Services.AddSwaggerGen(swagger =>
{
    swagger.SwaggerDoc("v1", new Microsoft.OpenApi.Models.OpenApiInfo
    {
        Version = "v1",
        Title = "API Bancária",
        Description = "Uma API bancária que permite a movimentação financeira (crédito e débito) de contas correntes, assim como a consulta de saldo da conta.",
        Contact = new OpenApiContact
        {
            Name = "José Janderson Eusébio da Silva de Oliveira",
            Email = "jandersoneusebio.dev@gmail.com"
        }
    });

    var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
    var path = Path.Combine(AppContext.BaseDirectory, xmlFile);

    swagger.IncludeXmlComments(path);
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();

    



}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

// sqlite
#pragma warning disable CS8602 // Dereference of a possibly null reference.
app.Services.GetService<IDatabaseBootstrap>().Setup();
#pragma warning restore CS8602 // Dereference of a possibly null reference.

app.Run();

// Informações úteis:
// Tipos do Sqlite - https://www.sqlite.org/datatype3.html


