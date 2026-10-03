namespace SimpleDB.tests;

using SimpleDB;

public class UnitTest1
{
    [Fact]
    public void CSVDatabaseDoesNotStoreCommentToNonexistingObservation()
    {
        //Arange
        long timestamp = DateTimeOffset.Now.ToUnixTimeSeconds() + 7200;
        Comment comment = new Comment("teklasvane","sej fugl", timestamp,99);
        CSVDatabase<Comment> database = CSVDatabase<Comment>.getInstance();
        
        //Act
        long before = database.GetNumberOfLinesInAFile("test_comment.csv");
        database.StoreComment(comment, "test_comment.csv", "test_observation.csv", 99);
        long after = database.GetNumberOfLinesInAFile("test_comment.csv");
        
        //Assert
        Assert.Equal(before, after);
    }

    [Fact]
    public void CSVDatabaseStoresCommentToExistingObservation()
    {
        //Arange
        long timestamp = DateTimeOffset.Now.ToUnixTimeSeconds() + 7200;
        string author = "teklasvane";

        Observations observation = new Observations(author, "her er en sej fugl!", timestamp, 0, "DR Byen");
        CSVDatabase<Observations> observationsDatabase = CSVDatabase<Observations>.getInstance();

        Comment comment = new Comment(author,"sej fugl", timestamp,0);
        CSVDatabase<Comment> commentDatabase = CSVDatabase<Comment>.getInstance();
        
        //Act
        long beforeObservation = observationsDatabase.GetNumberOfLinesInAFile("test_observation.csv");
        long beforeComment = commentDatabase.GetNumberOfLinesInAFile("test_comment.csv");

        observationsDatabase.Store(observation, "test_observation.csv", "DR Byen");
        commentDatabase.StoreComment(comment, "test_observation.csv", "test_comment.csv", 0);

        long afterObservation = observationsDatabase.GetNumberOfLinesInAFile("test_observation.csv");
        long afterComment = commentDatabase.GetNumberOfLinesInAFile("test_comment.csv");


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

    [Fact]
    public void CSVDatabaseDoesNotStoreProposalToNonexistingObservation()
    {
       //Arange
        long timestamp = DateTimeOffset.Now.ToUnixTimeSeconds() + 7200;
        Proposal proposal = new Proposal("raaminraza","cute penguin", timestamp,88);
        CSVDatabase<Proposal> database = CSVDatabase<Proposal>.getInstance();
        
        //Act
        long before = database.GetNumberOfLinesInAFile("test_proposal.csv");
        database.StoreProposal(proposal, "test_proposal.csv", "test_observation.csv", 88);
        long after = database.GetNumberOfLinesInAFile("test_proposal.csv");
        
        //Assert
        Assert.Equal(before, after); 
    }
}