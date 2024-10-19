namespace UsageCollectorWorkerService.Services;

public interface IGetSystemResources
{
    Task<byte> GetCpuUsageInPercent();
    Task<byte> GetRamUsageInPercent();
}