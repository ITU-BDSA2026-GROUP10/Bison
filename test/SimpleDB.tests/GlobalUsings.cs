namespace SimpleDB.tests;

using System.Reflection;
global using Xunit;
using System.Transactions;

public class GlobalUsings
{
    [Fact]
    public void storedCommentsCanBeRetrived ()
    {
        CSVDatabase csvDatabase = new CSVDatabase ();
        long timestamp = DateTimeOffset.Now.ToUnixTimeSeconds() + 7200;
        Comment comment = new Comment("sofiehelt","spotted heron a DR byen", timestamp,1);
        csvDatabase.storeComment(observation,"integrationstest.csv","spotted heron at DR byen",1);
        List <Comment> comment = csvDatabase.read("integrationstest_comment.csv");

        Assert.Contains(comment, c => c.ObservationId == 1 && c.Author == "sofiehelt" && c.Timestamp == timestamp 
        && c.Observation == "spotted heron at DR byen");

    }

    [Fact]
    public void storedObservationsCanBeRetrived ()
    {
        CSVDatabase csvDatabase = new CSVDatabase ();
        long timestamp = DateTimeOffset.Now.ToUnixTimeSeconds() + 7200;
        List <Observation> observationsToBeStored = new List<Observation>();
        Observation observation = new Observation(ropf,"A bird at DR Byen",1690891760,0);
        Observation observation = new Observation(adho,"I think that is a Heron at DR Byen", 1690978778,1);
        Observation observation = new Observation(edka,"Ardea cinerea at DR Byen",1690979858,2);
        foreach (observation o in observationsToBeStored)
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