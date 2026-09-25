using System.CommandLine;

namespace Bison.CLI.tests;

public class UnitTest1
{
    [Fact]
    public void ReadCommandExists()
    {
        var rootCommand = new RootCommand();
        var readCommand = new Command("read");
        rootCommand.Add(readCommand);
        var result = rootCommand.Parse("read");
        Assert.Empty(result.Errors);
    }

    [Fact]
    public void LocationCommandParsesArgument()
    {
        var locationArgument = new Argument<string>("location");
        var command = new Command("location");
        command.Add(locationArgument);
        var result = command.Parse("Aarhus");
        Assert.Equal("Aarhus", result.GetValue(locationArgument));
    }

    [Fact]
    public void ObserveCommandParsesArgument()
    {
        var observationArgument = new Argument<string>("observation");
        var locationArgument = new Argument<string>("location");
        var command = new Command("observation","location");
        command.Add(locationArgument);
        command.Add(observationArgument);
        var result = command.Parse("Heron Ismageriet");
        Assert.Empty(result.Errors);
    }

     [Fact]
    public void UnixTimeConvertsCorrectlyToUserReadableTime()
    {
        //Arrange
        long unixTime = 1789313646+7200;

        //Act
        DateTime dateTime = UserInterface.convertTime(unixTime);
        string dateTimeString = dateTime.ToString();
        
        //Assert
        Assert.Equal("13/09/2026 17.34.06", dateTimeString);
    }
}