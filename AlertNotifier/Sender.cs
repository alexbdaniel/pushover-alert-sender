using System.Net.Http.Headers;
using System.Security.Authentication;
using System.Text;
using System.Text.Json;
using Microsoft.Extensions.Options;

namespace AlertNotifier;

public class Sender
{
    private readonly SenderOptions options;

    private readonly JsonSerializerOptions serializerOptions = new()
    {
        PropertyNamingPolicy = JsonNamingPolicy.SnakeCaseLower,
        IncludeFields = true
    };

    public Sender(IOptions<SenderOptions> options)
    {
        this.options = options.Value;
    }

    public async Task SendAsync(RequestArgs args)
    {
        var handler = new HttpClientHandler
        {
            PreAuthenticate = true,
            SslProtocols = SslProtocols.Tls13
        };
        
        using var client = new HttpClient(handler);
        client.BaseAddress = new Uri(options.BaseAddress);
        client.DefaultRequestHeaders.Add("token", options.BearerToken);
        client.DefaultRequestHeaders.Add("user", options.UserId);
        
        string rawContent = JsonSerializer.Serialize(args, serializerOptions);
        var content = new StringContent(rawContent, Encoding.UTF8, "application/json");

        var response = await client.PostAsync(options.Endpoint, content);

        response.EnsureSuccessStatusCode();
    }
}