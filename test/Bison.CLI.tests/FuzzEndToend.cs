global using Xunit;


using System.CommandLine;
using Bison.CLI;
using System.Net;
using System.Net.Http;
using Microsoft.VisualBasic;
using SimpleDB;
using System.Collections;
using System.Collections.Generic;
using Xunit.Sdk;

public class FuzzEndToEndTest
{
    string randString;
    string randID; //observationID
    string [] arg;
    Dictionary <string[],string[]> dictionaryOracle = new Dictionary<string[], string[]>();
    List <string[]> argumentsObserve = new List<string[]>();
    List <string[]> argumentsProposal = new List<string[]>();
    List <string[]> argumentsComment = new List<string[]>();
    List <string> addedIdentifier = new List<string>();
    string command;
    
    public string[] generateArguments (string command) {
        Random rand = new Random();
        var rootCommand = new RootCommand();
        if (command.Equals("observe"))
        {
            arg = [generateString(), generateString()];
            argumentsObserve.Add(arg);
            addedIdentifier.Add(arg[0]);
            dictionaryOracle = getParsedValues(arg,dictionaryOracle);
        } else if (command.Equals("propals"))
        {
            string id = rand.Next(0,100).ToString();
            addedIdentifier.Add(id);
            arg = [generateString(),id];
            argumentsProposal.Add(arg);
            dictionaryOracle = getParsedValues(arg,dictionaryOracle);
        } else
        {
            string id = rand.Next(0,100).ToString();
            addedIdentifier.Add(id);
            arg = [generateString(), id];
            argumentsComment.Add(arg);
            dictionaryOracle = getParsedValues(arg,dictionaryOracle);
        }

        return arg;
    }

    public Dictionary<string[],string[]> getParsedValues (string [] arg, Dictionary<string[],string[]> dictionaryOracle)
    {
        var Argument1 = new Argument<string>(arg[0]);
        var Argument2 = new Argument<string>(arg[1]);
        var command = new Command(arg[0],arg[1]);
        command.Add(Argument1);
        command.Add(Argument2);
        string seq = arg[0] + arg[1];
        var result = command.Parse(seq);
        string[] actual = [result.GetValue(Argument1), result.GetValue(Argument2)];
        dictionaryOracle.Add(arg,actual);
        return dictionaryOracle;
    }

    [Fact]
    public async Task ObservationsRand ()
    {
        for (int i = 0; i < 100 ; i++)
        {
            //Arrange
            arg = generateArguments("observe");

            //Act
            int result = await Program.Main(arg);

            Task <int> exitcode = new Task<int>(() => 0);
            exitcode.Start();
            await exitcode;

            //Assert
            Assert.Equal(0, result);
        }
    }

    [Fact]
    public async Task ProposalsRand ()
    {
        for (int i = 0; i < 100 ; i++)
        {
            //Arrange
            arg = generateArguments("proposals");

            //Act
            int result = await Program.Main(arg);

            Task <int> exitcode = new Task<int>(() => 0);
            exitcode.Start();
            await exitcode;
            // dictionaryOracle.Add(0,result);

            //Assert
            Assert.Equal(0, result);
        }
    }

    [Fact]
    public async Task CommentRand ()
    {
        for (int i = 0; i < 100 ; i++)
        {
            //Arrange
            arg = generateArguments("comment");

            //Act
            int result = await Program.Main(arg);

            Task <int> exitcode = new Task<int>(() => 0);
            exitcode.Start();
            await exitcode;
            // dictionaryOracle.Add(0,result);

            //Assert
            Assert.Equal(0, result);
        }
    }

    public void testOracle (string [] argument, string[] expected)
    {
        dictionaryOracle.Add(argument,expected);
    }

    //https://www.geeksforgeeks.org/c-sharp/c-sharp-randomly-generating-strings/
    public static string generateString ()
    {
        Random rand = new Random();

        int stringlen = rand.Next(0, 10);
        int randValue;
        string str = "";
        char letter;
        for (int i = 0; i < stringlen; i++)
        {
        randValue = rand.Next(0, 26);
        letter = Convert.ToChar(randValue + 65);
        str = str + letter;
        }
        return str;
    }

    public void cleanUpCsv (List <string> addedIdentifier, string command)
    {
        foreach (string id in addedIdentifier){
            if(command.Equals("observe"))
            {
                CSVDatabase<Observations> database = CSVDatabase<Observations>.getInstance();
                List<string> observerecords = database.ReadObservation("../Bison.CLI/bison_observe_cli_db.csv").ToList<string>();
                foreach (Observations obs in observerecords)
                {
                    if (obs.Contains(id))
                    {
                        //remove line
                    }
                }
            } else if (command.Equals("proposal"))
            {
                CSVDatabase<Taxon> database = CSVDatabase<Taxon>.getInstance();
                List<Taxon> taxonrecords = database.ReadProposals("../Bison.CLI/bison_proposal_cli_db.csv", long.Parse(id)).ToList<Taxon>();
            } else
            {
                CSVDatabase<Comment> database = CSVDatabase<Comment>.getInstance();
                List<Comment> taxonrecords = database.ReadDiscussion("../Bison.CLI/bison_comment_cli_db.csv", long.Parse(id)).ToList<Comment>();
            }
            }
        }
    }
}