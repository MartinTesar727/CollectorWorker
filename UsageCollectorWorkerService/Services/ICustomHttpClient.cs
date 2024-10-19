using UsageCollectorWorkerService.Models;

namespace UsageCollectorWorkerService.Services;

public interface ICustomHttpClient
{
    Task<HttpResponseMessage> PostRequestAsync(RootUsageValues rootUsageValues);
}