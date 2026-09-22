namespace Bison.CLI.tests;
using SimpleDB;

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
        Assert.Equal("13-09-2026 17:34:06", dateTime.ToString());
    }

    /*[Fact]
    public void CommentToNonExistingObservationReturnsFalse()
    {
        //Arrange
        Comment comment = new Comment(Environment.UserName, "This is a test comment", DateTimeOffset.Now.ToUnixTimeSeconds() + 7200, 10);
        //Act

        //Assert
        Assert.False(UserInterface.ObservationExists(2, comment.ObservationId));   
    }*/
}