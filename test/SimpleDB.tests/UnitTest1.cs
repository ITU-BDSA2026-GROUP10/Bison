namespace SimpleDB.tests;

using System.ComponentModel.Design;
using System.Reflection;
using SimpleDB;

public class UnitTest1
{   
    [Fact]
    public void TestsRecordHierarchy()
    {
        // Arange
        Cheep cheep = new Cheep(Environment.UserName, "testing...", 22);
        Observations obs = new Observations(Environment.UserName, "testing obs...", 22, 101, "ITU");
        Observations obs1 = new Observations(Environment.UserName, "testing obs...", 22, 101, "ITU");
        
        // Act
    
        // Assert
        Assert.True(obs is Cheep);
        Assert.False(cheep is Observations);
        Assert.False(cheep == obs);
        Assert.True(obs == obs1);
    }

}