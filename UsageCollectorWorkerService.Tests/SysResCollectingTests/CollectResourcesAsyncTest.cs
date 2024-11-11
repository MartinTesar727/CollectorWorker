using Moq;
using UsageCollectorWorkerService.Services.LowLevelCollecting;
using UsageCollectorWorkerService.Services.SystemResourcesCollector;
using UsageCollectorWorkerService.Validators;
using Xunit.Abstractions;
using FluentValidation;
using UsageCollectorWorkerService.Services.DataHolder;

namespace UsageCollectorWorkerService.Tests.SysResCollectingTests;

public class CollectResourcesAsyncTest : TestBase
{
    public CollectResourcesAsyncTest(ITestOutputHelper output) : base(output)
    { 
    }

    [Theory]
    [InlineData(-50, 50)]
    [InlineData(150, 50)]
    [InlineData(50, -50)]
    [InlineData(50, 150)]
    [InlineData(-50, -50)]
    [InlineData(150, 150)]
    public async void CollectResourcesAsync_CollectsInvalidArguments_ExceptionIsThrowned(int cpuPercentage, int ramPercentage)
    {
        //Arrange
        Mock<ILowLevelCollectingSevice> lowLevelCollectorMock = new Mock<ILowLevelCollectingSevice>();
        lowLevelCollectorMock.Setup(s => s.GetCpuUsageInPercentAsync()).Returns(Task.FromResult(cpuPercentage));
        lowLevelCollectorMock.Setup(s => s.GetRamUsageInPercentAsync()).Returns(Task.FromResult(ramPercentage));

        var dataHolderService = new DataHolderService();

        SysResCollectingService collector = new SysResCollectingService(
            new SysResUsageValidator(),
            lowLevelCollectorMock.Object,
            dataHolderService
        );

        //Act
        ValidationException result = await Assert.ThrowsAsync<ValidationException>(() =>
        {
            return collector.CollectResourcesAsync(5, 1);
        });

        //Assert
        Assert.Multiple(() =>
        {
            if (cpuPercentage is < 0 or > 100)
            {
                Assert.Contains("CPU usage must be between 0 and 100%", result.Message);
            }

            if (ramPercentage is < 0 or > 100)
            {
                Assert.Contains("RAM usage must be between 0 and 100%", result.Message);
            }
        });
    }

    [Theory]
    [InlineData(50, 50)]
    [InlineData(0, 0)]
    [InlineData(100, 100)]
    public async void CollectResourcesAsync_CollectsValidArguments_MethodIsExecutedSuccessfully(int cpuPercentage, int ramPercentage)
    {
        //Arrange
        Mock<ILowLevelCollectingSevice> lowLevelCollectorMock = new Mock<ILowLevelCollectingSevice>();
        lowLevelCollectorMock.Setup(s => s.GetCpuUsageInPercentAsync()).Returns(Task.FromResult(cpuPercentage));
        lowLevelCollectorMock.Setup(s => s.GetRamUsageInPercentAsync()).Returns(Task.FromResult(ramPercentage));
        
        var dataHolderService = new DataHolderService();

        SysResCollectingService collector = new SysResCollectingService(
            new SysResUsageValidator(),
            lowLevelCollectorMock.Object,
            dataHolderService
            );
        
        //Act
        var result = await collector.CollectResourcesAsync(3, 1);
        
        //Assert
        Assert.NotEmpty(result);
    }
}