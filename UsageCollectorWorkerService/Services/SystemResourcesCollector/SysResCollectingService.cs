using FluentValidation;
using UsageCollectorWorkerService.Models;
using UsageCollectorWorkerService.Services.LowLevelCollecting;

namespace UsageCollectorWorkerService.Services.SystemResourcesCollector;

public class SysResCollectingService : ISysResCollectingService
{
    private readonly ILowLevelCollectingSevice _lowLevelCollector;
    private readonly IValidator<SysResUsageValues> _gatheredValuesValidator;
    
    public SysResCollectingService(IValidator<SysResUsageValues> gatheredValuesValidator, ILowLevelCollectingSevice lowLevelCollector)
    {
        _lowLevelCollector = lowLevelCollector;
        _gatheredValuesValidator = gatheredValuesValidator;
    }

    public async Task<List<SysResUsageValues>> CollectResourcesAsync(int durationOfCollectingInSeconds, int intervalBetweenCollectingInSeconds)
    {
        DateTime endTimeOfCollecting = DateTime.Now.AddSeconds(durationOfCollectingInSeconds);
     
        List<SysResUsageValues> readings = new();

        while (DateTime.Now < endTimeOfCollecting)
        {
            Task<int> cpu = _lowLevelCollector.GetCpuUsageInPercentAsync();
            Task<int> ram = _lowLevelCollector.GetRamUsageInPercentAsync();

            await Task.WhenAll( cpu, ram );

            SysResUsageValues savedValue = new ()
            {
                UtcTimeStamp = DateTime.UtcNow.Ticks,
                CpuUsageInPercentage = cpu.Result,
                RamUsageInPercentage = ram.Result
            };
            
            _gatheredValuesValidator.ValidateAndThrow(savedValue);
            readings.Add(savedValue);
            
            await Task.Delay(TimeSpan.FromSeconds(intervalBetweenCollectingInSeconds));
        }
        
        return readings;
    }
}