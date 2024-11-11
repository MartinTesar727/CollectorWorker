using UsageCollectorWorkerService.Models;

namespace UsageCollectorWorkerService.Services.DataHolder;

public class DataHolderService : IDataHolderService
{
    private readonly List<SysResUsageValues> _values;
    
    public DataHolderService()
    {
        _values = new List<SysResUsageValues>();
    }

    public List<SysResUsageValues> GetValues()
    {
        return _values;
    }
    
    public void InsertValue(SysResUsageValues input)
    {
        _values.Add(input);
    }

    public void DeleteValues()
    {
        _values.Clear();
    }
}