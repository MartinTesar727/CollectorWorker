using Moq;
using UsageCollectorWorkerService.Services.LowLevelCollecting;
using UsageCollectorWorkerService.Services.SystemResourcesCollector;
using UsageCollectorWorkerService.Validators;
using Xunit.Abstractions;
using FluentValidation;

namespace UsageCollectorWorkerService.Tests.SysResCollectingTests;

public class CollectResourcesAsyncTest : TestBase
{
    private ISysResCollectingService _collector;
    private Mock<ILowLevelCollectingSevice> _lowLevelCollectorMock;
    
    public CollectResourcesAsyncTest(ITestOutputHelper output) : base(output)
    {
        _lowLevelCollectorMock = new Mock<ILowLevelCollectingSevice>();
    }

    [Theory]
    [InlineData(-50, -50)]
    [InlineData(150, 150)]
    public async void CollectResourcesAsync_CollectsInvalidArguments_ExceptionIsThrowned(int cpuPercentage, int ramPercentage)
    {
        //Arrange
        _lowLevelCollectorMock.Setup(s => s.GetCpuUsageInPercentAsync()).Returns(Task.FromResult(cpuPercentage));
        _lowLevelCollectorMock.Setup(s => s.GetRamUsageInPercentAsync()).Returns(Task.FromResult(ramPercentage));

        _collector = new SysResCollectingService(new SysResUsageValidator(), _lowLevelCollectorMock.Object);
        
        //Act
        ValidationException result = await Assert.ThrowsAsync<ValidationException>(() =>
        {
            return _collector.CollectResourcesAsync(5, 1);
        });
        
        //Assert
        Assert.Multiple(() =>
        {
            Assert.Contains("CPU usage must be between 0 and 100%", result.Message);
            Assert.Contains("RAM usage must be between 0 and 100%", result.Message);
        });
    }
}