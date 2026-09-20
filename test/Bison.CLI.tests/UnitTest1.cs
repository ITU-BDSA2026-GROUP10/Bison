namespace Bison.CLI.tests;

public class UnitTest1
{
    [Fact]
    public void UnixTimeConvertsCorrectlyToUserReadableTime()
    {
        //Arrange
        long unixTime = 1789313646+7200;

        //Act
        DateTime dateTime = UserInterface.GetDateTime(unixTime);
        
        //Assert
        Assert.Equal("13/09/2026 17.34.06", dateTime.ToString());
    }
}