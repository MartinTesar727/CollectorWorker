namespace UsageCollectorWorkerService.Services.LowLevelCollecting;

public interface ILowLevelCollectingSevice
{
    Task<int> GetCpuUsageInPercentAsync();
    Task<int> GetRamUsageInPercentAsync();
}