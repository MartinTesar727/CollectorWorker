using UsageCollectorWorkerService.Services.DataHolder;
using UsageCollectorWorkerService.Services.DataSender;
using UsageCollectorWorkerService.Services.SystemResourcesCollector;

namespace UsageCollectorWorkerService.BackgroundWorkers;

public class CollectorWorker : BackgroundService
{
    private readonly IDataHolderService _dataHolderService;
    private readonly ISenderService _senderService;
    private readonly ISysResCollectingService _sysResCollectingService;
    private readonly int _durationOfCollectingInSeconds;
    private readonly int _intervalBetweenCollectingInSeconds;

    public CollectorWorker(
        IDataHolderService dataHolderService,
        ISenderService senderService,
        ISysResCollectingService sysResCollectingService,
        int durationOfCollectingInSeconds,
        int intervalBetweenCollectingInSeconds)
    {
        _dataHolderService = dataHolderService;
        _senderService = senderService;
        _sysResCollectingService = sysResCollectingService;
        _durationOfCollectingInSeconds = durationOfCollectingInSeconds;
        _intervalBetweenCollectingInSeconds = intervalBetweenCollectingInSeconds;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        prepis pomoci orchestratoru
        var dataHolderValues = _dataHolderService.GetValues();         
        
        while (!stoppingToken.IsCancellationRequested)
        {
            dataHolderValues = await _sysResCollectingService.CollectResourcesAsync(
                _durationOfCollectingInSeconds,
                _intervalBetweenCollectingInSeconds);
            
            await _senderService.PostRequestAsync(dataHolderValues);
            
            _dataHolderService.DeleteValues();
        }
    }
}