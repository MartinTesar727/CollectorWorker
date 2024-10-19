using System.Net;
using System.Text;
using UsageCollectorWorkerService.Models;
using System.Text.Json;

namespace UsageCollectorWorkerService.Services;

public class CustomHttpClient : ICustomHttpClient
{
    private readonly HttpClient _httpClient;
    
    public CustomHttpClient(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<HttpResponseMessage> PostRequestAsync(RootUsageValues rootUsageValues)
    {
        using HttpRequestMessage request = new();
        request.Method = HttpMethod.Post;
        request.Content = new StringContent(JsonSerializer.Serialize(rootUsageValues), Encoding.UTF8, "application/json");

        try
        {
            using HttpResponseMessage response = await _httpClient.SendAsync(request);
            return response;
        }
        catch (Exception ex)
        {
            return new HttpResponseMessage(HttpStatusCode.InternalServerError);
        }
    }
}