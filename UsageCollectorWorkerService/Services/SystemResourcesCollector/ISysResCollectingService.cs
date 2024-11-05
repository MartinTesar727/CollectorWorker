using UsageCollectorWorkerService.Models;

namespace UsageCollectorWorkerService.Services.SystemResourcesCollector;

public interface ISysResCollectingService
{
    Task<List<SysResUsageValues>> CollectResourcesAsync(int durationOfCollectingInSeconds, int intervalBetweenCollectingInSeconds);
}