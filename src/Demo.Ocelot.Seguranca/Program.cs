using Microsoft.OpenApi.Models;
using System.Globalization;

var builder = WebApplication.CreateBuilder(args);

// Adicionando serviços que vamos usar ao container
builder.Services.AddControllers(opcoes =>
{
    opcoes.ReturnHttpNotAcceptable = true;
});
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(opcoes =>
{
    opcoes.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "Demo.Ocelot.Seguranca",
        Description = "Apenas uma Demo para demonstrar como funciona a segurança com Ocelot",
        Version = "v1"
    });
});

//Adicionar todas as urls das rotas em caixa baixa
builder.Services.Configure<RouteOptions>(options =>
{
    options.LowercaseUrls = true;
});

// Usando os serviços
var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(opcoes =>
    {
        opcoes.SwaggerEndpoint("/swagger/v1/swagger.json", "Demo.Ocelot.Seguranca v1");
    });
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

// Rodando a aplicação
app.Run();