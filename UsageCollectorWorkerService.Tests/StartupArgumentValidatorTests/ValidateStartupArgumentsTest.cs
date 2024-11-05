using UsageCollectorWorkerService.Validators;
using Xunit.Abstractions;

namespace UsageCollectorWorkerService.Tests.StartupArgumentValidatorTests;

public class ValidateStartupArgumentsTest : TestBase
{
    public ValidateStartupArgumentsTest(ITestOutputHelper output) : base(output)
    {
    }

    [Theory]
    [InlineData(new object[] {new string[] { "http://192.168.0.55:5000/api/resourceusage/save", "20", "5"}})]
    public void ValidateStartupArguments_ValidDataEntered_NoExceptionThrown(string[] cliArguments)
    {
        //Arrange
        //Act
        StartupArgumentsValidator.ValidateStartupArguments(cliArguments);
        //Assert
    }

    [Theory]
    [InlineData(new object[] {new string[] { "http://192.168.0.55:5000/api/resourceusage/save", "20", "5", "6"}})]
    [InlineData(new object[] {new string[] { "http://192.168.0.55:5000/api/resourceusage/save", "20"}})]
    [InlineData(new object[] {new string[] { }})]
    [InlineData(new object[] {null})]
    public void ValidateStartupArguments_IncorrectNumberOfArguments_ArgumentExceptionThrown(string[] cliArguments)
    {
        //Arrange
        //Act
        ArgumentException exception = Assert.Throws<ArgumentException>(() => StartupArgumentsValidator.ValidateStartupArguments(cliArguments));
        
        //Assert
        Assert.Equivalent("3 startup arguments are required", exception.Message);
    }
    
    [Theory]
    [InlineData(new object[] {new string[] { "http://192..168.0.55:5000/api/resourceusage/save", "20", "5"}})] // double dot
    [InlineData(new object[] {new string[] { "htt://192.168.0.55:5000/api/resourceusage/save", "20", "5"}})] // http
    [InlineData(new object[] {new string[] { "http://168.0.55:5000/api/resourceusage/save", "20", "5"}})] // 3 octetcs instead of 4
    public void ValidateStartupArguments_InvalidServerPath_ArgumentExceptionThrown(string[] cliArguments)
    {
        //Arrange
        //Act
        ArgumentException exception = Assert.Throws<ArgumentException>(() => StartupArgumentsValidator.ValidateStartupArguments(cliArguments));
        
        //Assert
        Assert.Equivalent("server path argument is invalid", exception.Message);
    }
    
    [Theory]
    [InlineData(new object[] {new string[] { "http://192.168.0.55:5000/api/resourceusage/save", "20.4568", "5"}})]
    [InlineData(new object[] {new string[] { "http://192.168.0.55:5000/api/resourceusage/save", "20", "5asd"}})]
    [InlineData(new object[] {new string[] { "http://192.168.0.55:5000/api/resourceusage/save", "2,0", "5"}})]
    [InlineData(new object[] {new string[] { "http://192.168.0.55:5000/api/resourceusage/save", "s", "5"}})]
    [InlineData(new object[] {new string[] { "http://192.168.0.55:5000/api/resourceusage/save", "-20", "5"}})]
    [InlineData(new object[] {new string[] { "http://192.168.0.55:5000/api/resourceusage/save", "20", "-5"}})]
    public void ValidateStartupArguments_DurationAndIntervalIsNotInteger_ArgumentExceptionThrown(string[] cliArguments)
    {
        //Arrange
        //Act
        ArgumentException exception = Assert.Throws<ArgumentException>(() => StartupArgumentsValidator.ValidateStartupArguments(cliArguments));
        
        //Assert
        Assert.Equivalent("duration and interval arguments must be positive integer", exception.Message);
    }
    
    [Theory]
    [InlineData(new object[] {new string[] { "http://192.168.0.55:5000/api/resourceusage/save", "9", "1"}})]
    [InlineData(new object[] {new string[] { "http://192.168.0.55:5000/api/resourceusage/save", "601", "1"}})]
    public void ValidateStartupArguments_CollectingTimeMustBeBetween10And600Seconds_ArgumentExceptionThrown(string[] cliArguments)
    {
        //Arrange
        //Act
        ArgumentException exception = Assert.Throws<ArgumentException>(() => StartupArgumentsValidator.ValidateStartupArguments(cliArguments));
        
        //Assert
        Assert.Equivalent("collecting time must be between 10 and 600 seconds", exception.Message);
    }
    
    [Theory]
    [InlineData(new object[] {new string[] { "http://192.168.0.55:5000/api/resourceusage/save", "60", "0"}})]
    [InlineData(new object[] {new string[] { "http://192.168.0.55:5000/api/resourceusage/save", "60", "61"}})]
    public void ValidateStartupArguments_IntervalBetweenCollectingMustBeBetween1And60Seconds_ArgumentExceptionThrown(string[] cliArguments)
    {
        //Arrange
        //Act
        ArgumentException exception = Assert.Throws<ArgumentException>(() => StartupArgumentsValidator.ValidateStartupArguments(cliArguments));
        
        //Assert
        Assert.Equivalent("interval between collecting must be between 1 and 60 seconds", exception.Message);
    }
    
    [Theory]
    [InlineData(new object[] {new string[] { "http://192.168.0.55:5000/api/resourceusage/save", "50", "50"}})]
    [InlineData(new object[] {new string[] { "http://192.168.0.55:5000/api/resourceusage/save", "50", "55"}})]
    public void ValidateStartupArguments_IntervalBetweenCollectingCannotBeBiggerThanCollectingTimeInTotal_ArgumentExceptionThrown(string[] cliArguments)
    {
        //Arrange
        //Act
        ArgumentException exception = Assert.Throws<ArgumentException>(() => StartupArgumentsValidator.ValidateStartupArguments(cliArguments));
        
        //Assert
        Assert.Equivalent("interval between collecting cannot be same or bigger than collecting time in total", exception.Message);
    }
}