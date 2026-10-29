namespace Bison.Razor.tests;

using Bison.Razor.BisonService;


public class UnixTimeTest
{
    [Fact]
    public void UnixTimeConvertsCorrectlyToUserReadableTime()
    {
        //Arrange
        long unixTime = 1789313646+7200;
        DateTime dateTime = new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);
        dateTime = dateTime.AddSeconds(unixTime);
        string um = dateTime.ToString("MM/dd/yy HH:mm:ss");
        um = um.Replace("-","/");
        string am = "09/13/26 17:34:06";
        
        
        //Act
        string dateTimeUnix = ObservationService.UnixTimeStampToDateTimeString(unixTime);
        dateTimeUnix = dateTimeUnix.Replace("-","/");

        //Assert
        Assert.Equal(am, um);
        Assert.Equal(am, dateTimeUnix);
    }
    
    [Fact]
    public void UnixTimeCorrectCharacterAmount()
    {
        //Arrange
        long unixTime = 1789313646+7200;
        string am = "09/13/26 17.34.06";
        
        //Act
        string dateTime = ObservationService.UnixTimeStampToDateTimeString(unixTime);

        //Assert
        Assert.Equal(am.Length, dateTime.Length);
    }
    
}