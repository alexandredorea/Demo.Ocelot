using Demo.Ocelot.Gateway.Agregadores;
using Demo.Ocelot.Gateway.Handlers;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Ocelot.DependencyInjection;
using Ocelot.Middleware;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

//Adicionar serviços ao container
builder.Configuration
    .AddJsonFile("ocelot.json", optional: false, reloadOnChange: true);

builder.Services
    .AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(opcoes =>
    {
        opcoes.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuerSigningKey = true,
            ValidateIssuer = false,
            ValidateAudience = false,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes("Minha-Chave-Secreta-de-Seguranca-Anonima")),
            ClockSkew = new TimeSpan(0)
        };
    });

builder.Services
    .AddOcelot(builder.Configuration)
    .AddDelegatingHandler<RemoveEncodingDelegatingHandler>(true)
    .AddDelegatingHandler<BlackListHandler>(true)
    .AddSingletonDefinedAggregator<PostsUsuariosAgregados>();

//Adicionar todas as urls das rotas em caixa baixa
builder.Services.Configure<RouteOptions>(options =>
{
    options.LowercaseUrls = true;
});

//Construir o container
var app = builder.Build();
app.UseAuthentication();
await app.UseOcelot();
//Rodar o container
app.Run();