using Moq;
using UsageCollectorWorkerService.BackgroundWorkers;
using UsageCollectorWorkerService.Models;
using UsageCollectorWorkerService.Services.DataHolder;
using UsageCollectorWorkerService.Services.DataSender;
using UsageCollectorWorkerService.Services.SystemResourcesCollector;
using Xunit.Abstractions;

namespace UsageCollectorWorkerService.Tests.CollectorWorkerTests;

public class WorkerTest : TestBase
{
    public WorkerTest(ITestOutputHelper output) : base(output)
    {
    }

    [Fact]
    public async void ExecuteAsync_CancellationTokenWorks_ServiceIsStopped()
    {
        //Arrange
        Mock<IDataHolderService> dataHolderServiceMock = new ();
        dataHolderServiceMock
            .Setup(m => m.Instance)
            .Returns(dataHolderServiceMock.Object.Instance = new RootSysResUsageValues(){UsageValues = new List<SysResUsageValues>()});
        
        Mock<ISenderService> senderServiceMock = new ();
        senderServiceMock
            .Setup(m => m.PostRequestAsync(dataHolderServiceMock.Object.Instance))
            .Returns(Task.CompletedTask);
        
        Mock<ISysResCollectingService> sysResCollectingServiceMock = new ();
        sysResCollectingServiceMock
            .Setup(m => m.CollectResourcesAsync(It.IsAny<int>(), It.IsAny<int>()))
            .Returns(Task<List<SysResUsageValues>>.FromResult(new List<SysResUsageValues>() {new SysResUsageValues() {CpuUsageInPercentage = 50, RamUsageInPercentage = 50, UtcTimeStamp = 4421574534} }));

        CollectorWorker worker = new (
            dataHolderServiceMock.Object,
            senderServiceMock.Object,
            sysResCollectingServiceMock.Object,
            60,
            1);
        
        //Act
        using CancellationTokenSource cts = new ();
        Task mainTask = worker.StartAsync(cts.Token);
        Task childTask = worker.ExecuteTask;
        
        await Task.Delay(TimeSpan.FromSeconds(3));
        cts.Cancel();
        await Task.Delay(TimeSpan.FromSeconds(3));
        
        //Assert
        Assert.True(childTask.Status is TaskStatus.Canceled);
    }
}