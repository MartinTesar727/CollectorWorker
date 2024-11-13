using UsageCollectorWorkerService.Orchestrators;

namespace UsageCollectorWorkerService.BackgroundWorkers;

public class CollectorWorker : BackgroundService
{
    private readonly IClientOrchestrator _clientOrchestrator;

    public CollectorWorker(IClientOrchestrator clientOrchestrator)
    {
        _clientOrchestrator = clientOrchestrator;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            await _clientOrchestrator.StartCollectingAsync();
        }
    }
}