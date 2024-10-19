namespace UsageCollectorWorkerService.Models;

public class UsageValues
{
    public long UtcTimeStamp { get; set; }
    public byte CpuUsageInPercentage { get; set; }
    public byte RamUsageInPercentage { get; set; }
}