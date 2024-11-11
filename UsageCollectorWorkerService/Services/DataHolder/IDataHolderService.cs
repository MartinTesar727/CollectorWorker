using UsageCollectorWorkerService.Models;

namespace UsageCollectorWorkerService.Services.DataHolder;

public interface IDataHolderService
{
    List<SysResUsageValues> GetValues();
    void InsertValue(SysResUsageValues input);
    void DeleteValues();
}