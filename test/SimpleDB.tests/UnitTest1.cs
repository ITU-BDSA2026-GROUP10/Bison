namespace SimpleDB.tests;

using System.ComponentModel.Design;
using System.Reflection;
using SimpleDB;

public class UnitTest1
{
    /*[Fact]
    public void CSVDatabaseDoesNotStoreCommentToNonexistingObservation()
    {
        //Arange
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
    */
    [Fact]
    public void TestName()
    {
        // Arange
        Cheep cheep = new Cheep(Environment.UserName, "testing...", 22);
        Observations obs = new Observations(Environment.UserName, "testing obs...", 22, 101);
        Observations obs1 = new Observations(Environment.UserName, "testing obs...", 22, 101);
        
        // Act
    
        // Assert
        Assert.True(obs is Cheep);
        Assert.False(cheep is Observations);
        Assert.False(cheep == obs);
        Assert.True(obs == obs1);
    }
}