using UsageCollectorWorkerService.Models;

namespace UsageCollectorWorkerService.Services.DataSender;

public interface ISenderService
{
    Task PostRequestAsync(RootSysResUsageValues rootSysResUsageValues);
}