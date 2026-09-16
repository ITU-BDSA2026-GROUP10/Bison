namespace SimpleDB.tests;

using System.Transactions;
using SimpleDB;

public class UnitTest1
{
    [Fact]
    public void CSVDatabaseDoesNotStoreCommentToNonexistingObservation()
    {
        //Arange
        string comment = "99, sej fugl";
        CSVDatabase<string> database = new CSVDatabase<string>();
        
        //Act
        long before = database.GetNumberOfLinesInAFile("test_comment_cli_db.csv");
        database.StoreComment(comment, "test_comment_cli_db.csv", "test_observe_cli_db.csv", 99);
        long after = database.GetNumberOfLinesInAFile("test_comment_cli_db.csv");
        
        //Assert
        Assert.Equal(before, after);
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
    public void storedCommentsCanBeRetrived ()
    {
        CSVDatabase<Comment> csvDatabase = new CSVDatabase <Comment> ();
        long timestamp = DateTimeOffset.Now.ToUnixTimeSeconds() + 7200;
        Comment comment = new Comment("sofiehelt","spotted heron a DR byen", timestamp,1);
        //csvDatabase.storeComment(observation,"integrationstest.csv","spotted heron at DR byen",1);
        //csvDatabase.StoreComment("spotted heron at DR byen","integrationstest.csv","integrationtest_observation",1);
        csvDatabase.StoreComment(comment,"integrationstest.csv","integrationtest_observation",1);
        //List <Comment> comments = csvDatabase.read("integrationstest_comment.csv");
        IEnumerable <Comment> comments = csvDatabase.read("integrationstest_comment.csv");

        Assert.Contains(comment, 
        c => c.ObservationId == 1 
        && c.Author == "sofiehelt" 
        && c.Timestamp == timestamp 
        && c.Observation == "spotted heron at DR byen");
    }

    [Fact]
    public void storedObservationsCanBeRetrived ()
    {
        CSVDatabase <Observation> csvDatabase = new CSVDatabase <Observation> ();
        long timestamp = DateTimeOffset.Now.ToUnixTimeSeconds() + 7200;
        List <Observation> observationsToBeStored = new List<Observation>();
        Observation observation0 = new Observation("ropf","A bird at DR Byen",1690891760,0);
        Observation observation1 = new Observation("adho","I think that is a Heron at DR Byen", 1690978778,1);
        Observation observation2 = new Observation("edka","Ardea cinerea at DR Byen",1690979858,2);
        
        //så vidt jeg kunne se blev observationerne aldrig tilføjet til listen, men ved ikke om det var meningen
        observationsToBeStored.Add(observation0);
        observationsToBeStored.Add(observation1);
        observationsToBeStored.Add(observation2);

        foreach (Observation o in observationsToBeStored)
        {
            csvDatabase.Store(o,"integrationstest_observation.csv");
        }
        List <Observation> observations = csvDatabase.read("integrationstest_observation.csv");

        foreach (Observation ob in observations)
        {
            AssemblyTrademarkAttribute.Contains(observations, i => i.Author = ob.Author && i.Observation == ob.Observation 
            && i.Timestamp == ob. Timestamp && i.ID == ob.ID);
        }
    }
}