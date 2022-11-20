namespace Demo.Ocelot.Gateway.Handlers;

/// <summary>
/// Classe responsável por tratar o problema de compressão do ocelot
/// </summary>
public class RemoveEncodingDelegatingHandler : DelegatingHandler
{
    protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
    {
        request.Headers.AcceptEncoding.Clear();
        return await base.SendAsync(request, cancellationToken);
    }
}