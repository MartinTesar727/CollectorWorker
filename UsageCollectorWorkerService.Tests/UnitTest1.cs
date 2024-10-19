using System.Collections;
using System.Diagnostics;
using UsageCollectorWorkerService.Services;
using Xunit.Abstractions;

namespace UsageCollectorWorkerService.Tests;

public class UnitTest1
{
    private readonly ITestOutputHelper _testOutputHelper;
    
    public UnitTest1(ITestOutputHelper output)
    {
        _testOutputHelper = output;
    }

    [Fact]
    public async void Test1()
    {
        
    }
}