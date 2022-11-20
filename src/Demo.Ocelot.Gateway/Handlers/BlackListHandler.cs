using System.Net;

namespace Demo.Ocelot.Gateway.Handlers;

public class BlackListHandler : DelegatingHandler
{
    protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
    {
        var cabecalho = request.Headers.FirstOrDefault(opcao => opcao.Key == "X-MeuCabecalho");

        if (cabecalho.Value != null && cabecalho.Value.Any())
            return await base.SendAsync(request, cancellationToken);

        var resposta = new HttpResponseMessage(HttpStatusCode.BadRequest)
        {
            ReasonPhrase = "Cabecalho invalido"
        };
        return await Task.FromResult(resposta);
    }
}