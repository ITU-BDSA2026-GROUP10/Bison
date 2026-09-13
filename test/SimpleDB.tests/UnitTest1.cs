namespace SimpleDB.tests;
using System.CommandLine;
using System.CommandLine.Parsing;

public class UnitTest1
{
    [Fact]
    public void CommentToNonexistingObservationThrowsAnException()
    {
        //Arange
        String comment = "99, sej fugl";
        Program program = new Program();
        Argument<string> commentArgument = new Argument<string>("comment");
        program.SetCommentAction("comment", commentArgument);
        
        //Act
        
        //Assert

    }
}