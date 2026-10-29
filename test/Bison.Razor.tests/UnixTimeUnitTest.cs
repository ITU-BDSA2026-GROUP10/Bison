namespace Bison.Razor.tests;

using Bison.Razor.BisonService;


public class UnixTimeTest
{
     
     [Fact]
    public void UnixTimeConvertsCorrectlyToUserReadableTime()
    {
        //Arrange
        long unixTime = 1789313646+7200;
        string am = "09/13/2026 17:34:06";

        //Act
        string dateTime = ObservationService.UnixTimeStampToDateTimeString(unixTime);
        
        //Assert
        Assert.Equal(am, dateTime);
    }
    
    [Fact]
    public void UnixTimeCorrectCharacterAmount()
    {
        //Arrange
        long unixTime = 1789313646+7200;
        string am = "09/13/2026 17:34:06";
        
        //Act
        string dateTime = ObservationService.UnixTimeStampToDateTimeString(unixTime);

        //Assert
        Assert.Equal(am.Length, dateTime.Length);
    }
    
}