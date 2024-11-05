using Xunit.Abstractions;

namespace UsageCollectorWorkerService.Tests;

public abstract class TestBase
{
    protected readonly ITestOutputHelper Output;
    
    protected TestBase(ITestOutputHelper output)
    {
        Output = output;
    }
}