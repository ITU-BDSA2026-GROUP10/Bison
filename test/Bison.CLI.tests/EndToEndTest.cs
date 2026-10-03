global using Xunit;


using System.CommandLine;
using Bison.CLI;
using System.Net;
using System.Net.Http;


public class EndtoEndTest
{
   [Fact]
   public async Task ProgramExistsCorrectlyAfterObserveCommand ()
   {
       //Arrange
       var args = new string[] {"observe", "Heron DR Byen", "DR Byen"};


       //Act
       int result = await Program.Main(args);


       Task <int> exitcode = new Task<int>(() => 0);
       exitcode.Start();
       await exitcode;
      
       //Assert
       Assert.Equal(0, result);
   }


[Fact]
public async Task ProgramExistsCorrectlyAfterReadCommand ()
   {
       //Arrange
       var args = new string[] {"read"};


       //Act
       int result = await Program.Main(args);


       Task <int> exitcode = new Task<int>(() => 0);
       exitcode.Start();
       await exitcode;
      
       //Assert
       Assert.Equal(0, result);
   }

/*[Fact]
public async Task ProgramReturnsCorrectOutputAfterReadCommand ()
{
    //Arrange
    var args = new string[] { "read" };

    var originalOutput = Console.Out;
    var output = new StringWriter();
    Console.SetOut(output);

    //Få Bison.CSVDBService til at bruge test_observation.csv i stedet 

    //Act
    await Program.Main(args);
    string result = output.ToString(); 

    //Assert

    //Sammenlign resultat med det forventede output fra test_observation.csv

    Console.SetOut(originalOutput); //sætter consolen tilbage til normal igen

}

[Fact]
public async Task ObserveCommandStoresPenguinInDatabase ()
{
    
}*/

}
