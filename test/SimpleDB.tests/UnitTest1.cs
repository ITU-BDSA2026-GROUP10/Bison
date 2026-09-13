namespace SimpleDB.tests;
using System.Transactions;

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
}