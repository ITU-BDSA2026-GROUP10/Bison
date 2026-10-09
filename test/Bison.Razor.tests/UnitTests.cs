namespace Bison.Razor.tests;
using System;
using System.Reflection;
using Bison.Razor.BisonService;

public class UnitTests
{
    [Fact]
    public void UnixTimeConvertsCorrectlyToUserReadableTime()
    {
        //Arrange
        long unixTime = 1789313646+7200;
        
        //Act
        string dateTime = ObservationService.UnixTimeStampToDateTimeString(unixTime);
        
        string am = "09/13/26 17.34.06";
        string dk = "09-13-26 17:34:06";
        string uk = "09.13.26 17.34.06";

        //Assert
        Assert.True(am == dateTime || dk == dateTime || uk == dateTime);
    }
}