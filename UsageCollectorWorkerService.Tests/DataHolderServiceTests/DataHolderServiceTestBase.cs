using UsageCollectorWorkerService.Services.DataHolder;
using Xunit.Abstractions;

namespace UsageCollectorWorkerService.Tests.DataHolderServiceTests;

public abstract class DataHolderServiceTestBase : TestBase 
{
    protected readonly IDataHolderService DataHolderService;
    
    protected DataHolderServiceTestBase(ITestOutputHelper output) : base(output)
    {
        DataHolderService = new DataHolderService();
    }
}