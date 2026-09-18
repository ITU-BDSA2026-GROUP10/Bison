namespace SimpleDB.tests;

using SimpleDB;

public class UnitTest1
{
    [Fact]
    public void CSVDatabaseDoesNotStoreCommentToNonexistingObservation()
    {
        //Arange
        //string comment = "99, sej fugl";
        long timestamp = DateTimeOffset.Now.ToUnixTimeSeconds() + 7200;
        Comment comment = new Comment("teklasvane","sej fugl", timestamp,99);
        CSVDatabase<Comment> database = new CSVDatabase<Comment>();
        
        //Act
        long before = database.GetNumberOfLinesInAFile("test_comment_cli_db.csv");
        database.StoreComment(comment, "test_comment_cli_db.csv", "test_observe_cli_db.csv", 99);
        long after = database.GetNumberOfLinesInAFile("test_comment_cli_db.csv");
        
        //Assert
        Assert.Equal(before, after);
    }

    [Fact]
    public void CSVDatabaseStoresCommentToExistingObservation()
    {
        //Arange
        long timestamp = DateTimeOffset.Now.ToUnixTimeSeconds() + 7200;
        string author = "teklasvane";

        Observations observation = new Observations(author, "her er en sej fugl!", timestamp, 0);
        CSVDatabase<Observations> observationsDatabase = new CSVDatabase<Observations>();

        Comment comment = new Comment(author,"sej fugl", timestamp,0);
        CSVDatabase<Comment> commentDatabase = new CSVDatabase<Comment>();
        
        //Act
        long beforeObservation = observationsDatabase.GetNumberOfLinesInAFile("test_observe_cli_db.csv");
        long beforeComment = commentDatabase.GetNumberOfLinesInAFile("test_comment_cli_db.csv");

        observationsDatabase.Store(observation, "test_observe_cli_db.csv");
        commentDatabase.StoreComment(comment, "test_comment_cli_db.csv", "test_observe_cli_db.csv", 0);

        long afterObservation = observationsDatabase.GetNumberOfLinesInAFile("test_observe_cli_db.csv");
        long afterComment = commentDatabase.GetNumberOfLinesInAFile("test_comment_cli_db.csv");

        //Assert
        Assert.NotEqual(beforeObservation, afterObservation);
        Assert.NotEqual(beforeComment, afterComment);
    }

    [Fact]
    public void UnixTimeConvertsCorrectlyToUserReadableTime()
    {
        //Arrange
        long unixTime = 1789313646+7200;

        //Act
        DateTime dateTime = DateTimeOffset.FromUnixTimeSeconds(unixTime).DateTime;
        string dateTimeString = dateTime.ToLongTimeString();
        
        //Assert
        Assert.Equal("17.34.06", dateTimeString);
    }
}