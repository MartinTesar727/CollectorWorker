using System.Text;
using System.Text.Json;
using UsageCollectorWorkerService.Models;

namespace UsageCollectorWorkerService.Services.DataHolder;

public class DataHolderService : IDataHolderService
{
    public DataHolderService()
    {
        Instance = new RootSysResUsageValues()
        {
            UsageValues = new List<SysResUsageValues>()
        };
    }

    public RootSysResUsageValues Instance { get; set; }
    
    public StringContent CreateStringContentForPostRequest(RootSysResUsageValues instance)
    {
        return new StringContent(JsonSerializer.Serialize(instance), Encoding.UTF8, "application/json");;
    }

    
}