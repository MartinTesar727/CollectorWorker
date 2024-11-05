namespace UsageCollectorWorkerService.Models;

public class SysResUsageValues
{
    public long UtcTimeStamp { get; set; }
    public int CpuUsageInPercentage { get; set; }
    public int RamUsageInPercentage { get; set; }
}