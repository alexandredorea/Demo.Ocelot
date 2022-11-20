using Demo.Ocelot.Gateway.Modelos;
using Ocelot.Middleware;
using Ocelot.Multiplexer;
using System.Net;
using System.Net.Http.Headers;
using System.Net.Mime;
using System.Text.Json;

namespace Demo.Ocelot.Gateway.Agregadores;

/// <summary>
/// Classe responsável por retorna uma agregação de dados de varios endpoints,
/// quando expômos publicamente um (criamos agregados) e não exibimos vários
/// gateways.
/// </summary>
public class PostsUsuariosAgregados : IDefinedAggregator
{
    /// <summary>
    /// Metodo recebe a resposta da requisição do ocelot, e cria o agregado para o endpoint solicitado.
    /// </summary>
    /// <param name="responses"></param>
    /// <returns></returns>
    public async Task<DownstreamResponse> Aggregate(List<HttpContext> responses)
    {
        var resultadoUsuarios = await responses[0].Items.DownstreamResponse().Content.ReadAsStringAsync();
        var resultadoPostagens = await responses[1].Items.DownstreamResponse().Content.ReadAsStringAsync();
        var opcoes = new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true
        };
        var usuarios = JsonSerializer.Deserialize<List<Usuario>>(resultadoUsuarios, opcoes);
        var postagens = JsonSerializer.Deserialize<List<Postagem>>(resultadoPostagens, opcoes);

        if (usuarios != null)
        {
            foreach (var usuario in usuarios)
            {
                var postagemUsuario = postagens?.Where(p => p.UserId == usuario?.Id).ToList();
                usuario?.Posts.AddRange(postagemUsuario!);
            }
        }

        var postagemUsuarioString = JsonSerializer.Serialize(usuarios);
        var conteudo = new StringContent(postagemUsuarioString)
        {
            Headers = { ContentType = new MediaTypeHeaderValue(MediaTypeNames.Application.Json) },
        };

        return new DownstreamResponse(conteudo, HttpStatusCode.OK, new List<KeyValuePair<string, IEnumerable<string>>>(), "OK");
    }
}