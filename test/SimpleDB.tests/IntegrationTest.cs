namespace SimpleDB.tests;

using System.Reflection;
using SimpleDB;
public class IntegrationTest
{
    /*[Fact]
    public void storedObservationsCanBeRetrieved ()
    {
        //Arrange
        CSVDatabase <Observations> csvDatabase = CSVDatabase<Observations>.getInstance();
        long timestamp = DateTimeOffset.Now.ToUnixTimeSeconds() + 7200;
        Observations observationToStore = new Observations(Environment.UserName,"A bird at DR Byen",timestamp,0, "ITU");
        
        string path = Path.GetTempFileName();
        
        //long linesInObservationFile = csvDatabase.GetNumberOfLinesInAFile(path); //"integrationstest_observation.csv"
        //Observations observationForAssert = new Observations(Environment.UserName, observationToStore.ToString(), timestamp, linesInObservationFile);
        
        //Act
        csvDatabase.Store(observationToStore, path, "ITU"); //"integrationstest_observation.csv"
        IEnumerable<Observations> observations = csvDatabase.ReadObservation(path); //"integrationstest_observation.csv"

        //Assert
        Assert.Contains(observationToStore, observations); 
    }*/
    /*[Fact]
    public void storedObservationsCanBeRetrieved2 ()
    {
        //Arrange
        CSVDatabase <Observations> csvDatabase = CSVDatabase<Observations>.getInstance();
        long timestamp = DateTimeOffset.Now.ToUnixTimeSeconds() + 7200;
        List <Observations> observationsToBeStored = new List<Observations>();
        Observations observationToStore = new Observations(Environment.UserName,"A bird at DR Byen",timestamp,0, "ITU");
        

        string path = Path.GetTempFileName();
        //long linesInObservationFile = csvDatabase.GetNumberOfLinesInAFile("test_observe_cli_db.csv");
        long linesInObservationFile = csvDatabase.GetNumberOfLinesInAFile("test_observation.csv"); //"integrationstest_observation.csv"
        Observations observationForAssert = new Observations(Environment.UserName, observationToStore.ToString(), timestamp, linesInObservationFile, "ITU");
        
        //Act
        //csvDatabase.Store(observationToStore,"test_observe_cli_db.csv");
        //IEnumerable<Observations> observations = csvDatabase.Read("test_observe_cli_db.csv");

        csvDatabase.Store(observationToStore, path, "ITU"); //"integrationstest_observation.csv"
        IEnumerable<Observations> observations = csvDatabase.ReadObservation(path); //"integrationstest_observation.csv"

        //Assert
        Assert.Contains(observationForAssert, observations); 
    }*/

    [Fact] 
    public void storedObservationsCanBeRetrieved2 ()
    {
        //Arrange
        CSVDatabase <Observations> csvDatabase = CSVDatabase<Observations>.getInstance();
        long timestamp = DateTimeOffset.Now.ToUnixTimeSeconds() + 7200;
        string author = "teklasvane";
        
        Observations observationToStore = new Observations(author,"A bird at DR Byen",timestamp,0,"ITU");

        string path = "test_observation.csv";
        
        //Act
        csvDatabase.Store(observationToStore, path, "ITU"); //"integrationstest_observation.csv"
        IEnumerable<Observations> observations = csvDatabase.ReadObservation(path); //"integrationstest_observation.csv"

        //Assert
        Assert.Contains(observationToStore, observations); 
    }

    /*[Fact]
    public void storedCommentsCanBeRetrieved ()
    {
        //Arrange
        CSVDatabase<Comment> csvDatabase = CSVDatabase<Comment>.getInstance();
        long timestamp = DateTimeOffset.Now.ToUnixTimeSeconds() + 7200;
        string author = "teklasvane";
        Comment commentToStore = new Comment(author, "spotted heron at DR byen", timestamp, 0);
        Comment commentForAssert = new Comment(author, commentToStore.ToString(), timestamp, 0);
        
        //Act
        //csvDatabase.StoreComment(commentToStore, "test_comment_cli_db.csv", "test_observe_cli_db.csv", 1);
        //IEnumerable <Comment> comments = csvDatabase.Read("test_comment_cli_db.csv");

        csvDatabase.StoreComment(commentToStore, "test_comment.csv", "test_observation.csv", 0);
        IEnumerable<Comment> comments = csvDatabase.ReadDiscussion("test_comment.csv", 0);
        
        //Assert
        Assert.Contains(commentForAssert, comments);
    }*/

    [Fact]
    public void storedCommentsCanBeRetrieved ()
    {
        //Arrange
        CSVDatabase<Comment> csvDatabase = CSVDatabase<Comment>.getInstance();
        long timestamp = DateTimeOffset.Now.ToUnixTimeSeconds() + 7200;
        string author = "teklasvane";

        Comment commentToStore = new Comment(author, "spotted heron at DR byen", timestamp, 0);
        //Comment commentForAssert = new Comment(author, commentToStore.ToString(), timestamp, 0);
        
        //Act
        csvDatabase.StoreComment(commentToStore, "test_observation.csv", "test_comment.csv", 0);
        IEnumerable<Comment> comments = csvDatabase.ReadDiscussion("test_comment.csv", 0);
        
        //Assert
        Assert.Contains(commentToStore, comments);
    }


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
    public void CSVDatabaseDoesNotStoreProposalToNonexistingObservation()
    {
       //Arange
        long timestamp = DateTimeOffset.Now.ToUnixTimeSeconds() + 7200;
        string taxonId = "MSTSNM:Arter:3e4e67e4-f785-ea11-aa77-501ac539d1ea";

        Proposal proposal = new Proposal("raaminraza", taxonId, timestamp, 88);

        CSVDatabase<Proposal> database = CSVDatabase<Proposal>.getInstance();
        database.ReadTaxon();
        
        //Act
        long before = database.GetNumberOfLinesInAFile("test_proposal.csv");
        database.StoreProposal(proposal, "test_proposal.csv", "test_observation.csv", 88);
        long after = database.GetNumberOfLinesInAFile("test_proposal.csv");
        
        //Assert
        Assert.Equal(before, after); 
    }

    [Fact]
    public void CSVDatabaseStoresProposalToExistingObservation()
    {
        //Arange
        CSVDatabase<Proposal> proposalDatabase = CSVDatabase<Proposal>.getInstance();
        proposalDatabase.ReadTaxon();

        long timestamp = DateTimeOffset.Now.ToUnixTimeSeconds() + 7200;
        string author = "raaminraza";
        string taxonId = "MSTSNM:Arter:3e4e67e4-f785-ea11-aa77-501ac539d1ea";

        Observations observation = new Observations(author, "her er en sej penguin!", timestamp, 0, "ITU");
        CSVDatabase<Observations> observationsDatabase = CSVDatabase<Observations>.getInstance();

        Proposal proposal = new Proposal(author,taxonId, timestamp,0);
        
        //Act
        long beforeObservation = observationsDatabase.GetNumberOfLinesInAFile("test_observation.csv");
        long beforeProposal = proposalDatabase.GetNumberOfLinesInAFile("test_proposal.csv");

        observationsDatabase.Store(observation, "test_observation.csv", "ITU");
        proposalDatabase.StoreProposal(proposal, "test_proposal.csv", "test_observation.csv", 0);

        long afterObservation = observationsDatabase.GetNumberOfLinesInAFile("test_observation.csv");
        long afterProposal = proposalDatabase.GetNumberOfLinesInAFile("test_proposal.csv");


        //Assert
        Assert.NotEqual(beforeObservation, afterObservation);
        Assert.NotEqual(beforeProposal, afterProposal);
    }

    [Fact]
    public void CSVDatabaseDoesNotStoreProposalWithInvalidTaxonId()
    {
       //Arange
        long timestamp = DateTimeOffset.Now.ToUnixTimeSeconds() + 7200;

        Proposal proposal = new Proposal("raaminraza", "invalidTaxonId", timestamp, 0);

        CSVDatabase<Proposal> database = CSVDatabase<Proposal>.getInstance();
        database.ReadTaxon();
        
        //Act
        long before = database.GetNumberOfLinesInAFile("test_proposal.csv");
        database.StoreProposal(proposal, "test_proposal.csv", "test_observation.csv", 0);
        long after = database.GetNumberOfLinesInAFile("test_proposal.csv");
        
        //Assert
        Assert.Equal(before, after); 
    }
}
