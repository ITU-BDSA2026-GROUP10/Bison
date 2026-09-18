namespace SimpleDB.tests;

using System.Reflection;
using SimpleDB;
public class IntegrationTest
{
    [Fact]
    public void storedCommentsCanBeRetrieved ()
    {
        //Arrange
        CSVDatabase<Comment> csvDatabase = new CSVDatabase <Comment> ();
        long timestamp = DateTimeOffset.Now.ToUnixTimeSeconds() + 7200;
        Comment commentToStore = new Comment(Environment.UserName, "spotted heron at DR byen", timestamp, 1);
        Comment commentForAssert = new Comment(Environment.UserName, commentToStore.ToString(), timestamp, 1);
        
        //Act
        csvDatabase.StoreComment(commentToStore, "test_comment_cli_db.csv", "test_observe_cli_db.csv", 1);
        IEnumerable <Comment> comments = csvDatabase.Read("test_comment_cli_db.csv");
        
        //Assert
        Assert.Contains(commentForAssert, comments);
    }

    [Fact]
    public void storedObservationsCanBeRetrieved ()
    {
        //Arrange
        CSVDatabase <Observations> csvDatabase = new CSVDatabase <Observations> ();
        long timestamp = DateTimeOffset.Now.ToUnixTimeSeconds() + 7200;
        List <Observations> observationsToBeStored = new List<Observations>();
        Observations observationToStore = new Observations(Environment.UserName,"A bird at DR Byen",timestamp,0);
        
        long linesInObservationFile = csvDatabase.GetNumberOfLinesInAFile("test_observe_cli_db.csv");
        Observations observationForAssert = new Observations(Environment.UserName, observationToStore.ToString(), timestamp, linesInObservationFile);
        
        //Act
        csvDatabase.Store(observationToStore,"test_observe_cli_db.csv");
        IEnumerable<Observations> observations = csvDatabase.Read("test_observe_cli_db.csv");

        //Assert
        Assert.Contains(observationForAssert, observations); 
    }
}