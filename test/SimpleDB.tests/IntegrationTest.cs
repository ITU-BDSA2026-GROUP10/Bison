namespace SimpleDB.tests;

using SimpleDB;
public class IntegrationTest
{
    [Fact]
    public void storedCommentsCanBeRetrived ()
    {
        CSVDatabase<Comment> csvDatabase = new CSVDatabase <Comment> ();
        long timestamp = DateTimeOffset.Now.ToUnixTimeSeconds() + 7200;
        Comment comment = new Comment("sofiehelt","spotted heron a DR byen", timestamp,1);
        //csvDatabase.storeComment(observation,"integrationstest.csv","spotted heron at DR byen",1);
        //csvDatabase.StoreComment("spotted heron at DR byen","integrationstest.csv","integrationtest_observation",1);
        
        //csvDatabase.StoreComment(comment,"integrationstest.csv","integrationtest_observation.csv",1); <= den gamle
        csvDatabase.StoreComment(comment, "test_comment_cli_db.csv", "test_observe_cli_db.csv", 1);

        //List <Comment> comments = csvDatabase.read("integrationstest_comment.csv");
        
        //IEnumerable <Comment> comments = csvDatabase.Read("integrationstest_comment.csv"); <= den gamle
        IEnumerable <Comment> comments = csvDatabase.Read("test_comment_cli_db.csv");

        Assert.Contains(comments, 
        c => c.ObservationId == 1 
        && c.Author == "sofiehelt" 
        && c.Timestamp == timestamp 
        && c.Observation == "spotted heron at DR byen");
    }

    [Fact]
    public void storedObservationsCanBeRetrived ()
    {
        CSVDatabase <Observations> csvDatabase = new CSVDatabase <Observations> ();
        long timestamp = DateTimeOffset.Now.ToUnixTimeSeconds() + 7200;
        List <Observations> observationsToBeStored = new List<Observations>();
        Observations observation0 = new Observations("ropf","A bird at DR Byen",1690891760,0);
        Observations observation1 = new Observations("adho","I think that is a Heron at DR Byen", 1690978778,1);
        Observations observation2 = new Observations("edka","Ardea cinerea at DR Byen",1690979858,2);
        
        //så vidt jeg kunne se blev observationerne aldrig tilføjet til listen, men ved ikke om det var meningen
        observationsToBeStored.Add(observation0);
        observationsToBeStored.Add(observation1);
        observationsToBeStored.Add(observation2);

        foreach (Observations o in observationsToBeStored)
        {
            csvDatabase.Store(o,"integrationstest_observation.csv");
        }
        IEnumerable<Observations> observations = csvDatabase.Read("integrationstest_observation.csv");

        foreach (Observations ob in observations)
        {
            Assert.Contains(observations, i => i.Author == ob.Author && i.Observation == ob.Observation 
            && i.Timestamp == ob. Timestamp && i.ID == ob.ID);
        }
    }
}