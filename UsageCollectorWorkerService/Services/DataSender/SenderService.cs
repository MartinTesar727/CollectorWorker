using System.Text;
using System.Text.Json;
using UsageCollectorWorkerService.Models;

namespace UsageCollectorWorkerService.Services.DataSender;

public class SenderService : ISenderService
{
    private readonly HttpClient _httpClient;
    
    public SenderService(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task PostRequestAsync(List<SysResUsageValues> sysResUsageValues)
    {
        using HttpRequestMessage request = new();
        request.Method = HttpMethod.Post;
        request.Content = CreateStringContentForPostRequest(sysResUsageValues);

        await _httpClient.SendAsync(request);
    }
    
    private StringContent CreateStringContentForPostRequest(List<SysResUsageValues> instance)
    {
        return new StringContent(JsonSerializer.Serialize(instance), Encoding.UTF8, "application/json");;
    }
}