using UsageCollectorWorkerService.Models;

namespace UsageCollectorWorkerService.Services.DataHolder;

public interface IDataHolderService
{
    RootSysResUsageValues Instance { get; set; }
    StringContent CreateStringContentForPostRequest(RootSysResUsageValues instance);
}