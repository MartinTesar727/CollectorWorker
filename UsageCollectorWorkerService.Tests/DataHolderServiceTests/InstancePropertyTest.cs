using UsageCollectorWorkerService.Models;
using Xunit.Abstractions;

namespace UsageCollectorWorkerService.Tests.DataHolderServiceTests;

public class InstancePropertyTest : DataHolderServiceTestBase
{
    public InstancePropertyTest(ITestOutputHelper output) : base(output)
    {
    }

    [Fact]
    public void InstanceProperty_PropertyIsCalled_PropertyIsInitialized()
    {
        //Arrange
        //Act
        RootSysResUsageValues rootObject = DataHolderService.Instance;

        //Assert
        Assert.Multiple(() =>
        {
            Assert.NotNull(rootObject);
            Assert.NotNull(rootObject.UsageValues);
        });
    }
    
    [Fact]
    public void InstanceProperty_PropertyIsCalled_PropertyInnerCollectionIsEmpty()
    {
        //Arrange
        //Act
        List<SysResUsageValues> usageValues = DataHolderService.Instance.UsageValues;

        //Assert
        Assert.Empty(usageValues);
    }
}