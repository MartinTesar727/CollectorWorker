using UsageCollectorWorkerService.Services.DataHolder;
using UsageCollectorWorkerService.Services.DataSender;
using UsageCollectorWorkerService.Services.SystemResourcesCollector;

namespace UsageCollectorWorkerService.Orchestrators;

public class ClientOrchestrator : IClientOrchestrator
{
    private readonly IDataHolderService _dataHolderService;
    private readonly ISenderService _senderService;
    private readonly ISysResCollectingService _sysResCollectingService;
    private readonly int _durationOfCollectingInSeconds;
    private readonly int _intervalBetweenCollectingInSeconds;
    
    public ClientOrchestrator(
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

    public async Task StartCollectingAsync()
    {
        var dataHolderValues = _dataHolderService.GetValues();         
        
        dataHolderValues = await _sysResCollectingService.CollectResourcesAsync(
            _durationOfCollectingInSeconds,
            _intervalBetweenCollectingInSeconds);
           
        await _senderService.PostRequestAsync(dataHolderValues);
           
        _dataHolderService.DeleteValues();
    }
}