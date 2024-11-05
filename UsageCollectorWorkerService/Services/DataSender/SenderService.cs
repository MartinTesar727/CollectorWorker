using UsageCollectorWorkerService.Models;
using UsageCollectorWorkerService.Services.DataHolder;

namespace UsageCollectorWorkerService.Services.DataSender;

public class SenderService : ISenderService
{
    private readonly HttpClient _httpClient;
    private readonly IDataHolderService _dataHolderService;
    
    public SenderService(HttpClient httpClient, IDataHolderService dataHolderService)
    {
        _dataHolderService = dataHolderService;
        _httpClient = httpClient;
    }

    public async Task PostRequestAsync(RootSysResUsageValues rootSysResUsageValues)
    {
        using HttpRequestMessage request = new();
        request.Method = HttpMethod.Post;
        request.Content = _dataHolderService.CreateStringContentForPostRequest(rootSysResUsageValues);

        try
        {
            await _httpClient.SendAsync(request);
        }
        catch (Exception ex)
        {
            
        }
    }
}