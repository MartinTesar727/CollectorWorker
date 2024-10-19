using UsageCollectorWorkerService.Models;
using UsageCollectorWorkerService.Services;

namespace UsageCollectorWorkerService.BackgroundWorkers;

public class Worker : BackgroundService
{
    private readonly ILogger<Worker> _logger;
    private readonly ICustomHttpClient _httpClient;
    private readonly IGetSystemResources _getSystemResources;

    public Worker(ILogger<Worker> logger, ICustomHttpClient httpClient, IGetSystemResources getSystemResources)
    {
        _logger = logger;
        _httpClient = httpClient;
        _getSystemResources = getSystemResources;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        int i = 0;
        
        RootUsageValues valuesToBeSent = new()
        {
            UsageValues = new List<UsageValues>()
        };
        
        while (!stoppingToken.IsCancellationRequested)
        {
            i++;
            
            Task<byte> x = _getSystemResources.GetCpuUsageInPercent();
            Task<byte> y = _getSystemResources.GetRamUsageInPercent();

            valuesToBeSent.UsageValues.Add(new UsageValues()
            {
                UtcTimeStamp = DateTime.UtcNow.Ticks,
                CpuUsageInPercentage = await x,
                RamUsageInPercentage = await y
            });
            
            /*if (i >= 60)*/
            if (i >= 5)
            {
                using HttpResponseMessage response = await _httpClient.PostRequestAsync(valuesToBeSent);
                i = 0;
                valuesToBeSent.UsageValues.Clear();
            }
            
            await Task.Delay(1000, stoppingToken);
        }
    }
}