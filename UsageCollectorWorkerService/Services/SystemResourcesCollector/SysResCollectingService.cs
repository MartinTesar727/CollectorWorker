using FluentValidation;
using UsageCollectorWorkerService.Models;
using UsageCollectorWorkerService.Services.DataHolder;
using UsageCollectorWorkerService.Services.LowLevelCollecting;

namespace UsageCollectorWorkerService.Services.SystemResourcesCollector;

public class SysResCollectingService : ISysResCollectingService
{
    private readonly ILowLevelCollectingSevice _lowLevelCollector;
    private readonly IValidator<SysResUsageValues> _gatheredValuesValidator;
    private readonly IDataHolderService _dataHolderService;
    
    public SysResCollectingService(
        IValidator<SysResUsageValues> gatheredValuesValidator,
        ILowLevelCollectingSevice lowLevelCollector,
        IDataHolderService dataHolderService)
    {
        _gatheredValuesValidator = gatheredValuesValidator;
        _lowLevelCollector = lowLevelCollector;
        _dataHolderService = dataHolderService;
    }

    public async Task<List<SysResUsageValues>> CollectResourcesAsync(int durationOfCollectingInSeconds, int intervalBetweenCollectingInSeconds)
    {
        while (DateTime.Now < GetEndTimeOfCollecting(durationOfCollectingInSeconds))
        {
            var cpu = _lowLevelCollector.GetCpuUsageInPercentAsync();
            var ram = _lowLevelCollector.GetRamUsageInPercentAsync();

            await Task.WhenAll( cpu, ram );

            SysResUsageValues savedValue = new ()
            {
                UtcTimeStamp = DateTime.UtcNow.Ticks,
                CpuUsageInPercentage = cpu.Result,
                RamUsageInPercentage = ram.Result
            };
            
            await _gatheredValuesValidator.ValidateAndThrowAsync(savedValue);
            _dataHolderService.InsertValue(savedValue);
            
            await Task.Delay(TimeSpan.FromSeconds(intervalBetweenCollectingInSeconds));
        }
        
        return _dataHolderService.GetValues();
    }

    private DateTime GetEndTimeOfCollecting(int durationOfCollectingInSeconds)
    {
        return DateTime.Now.AddSeconds(durationOfCollectingInSeconds);
    }
}