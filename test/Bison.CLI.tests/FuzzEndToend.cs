global using Xunit;
global using SimpleDB;
using CsvHelper;
using System.CommandLine;
using Bison.CLI;
using System.Net;
using System.Net.Http;
using Microsoft.VisualBasic;
using System.Collections;
using System.Collections.Generic;
using Xunit.Sdk;
using System.Runtime.CompilerServices;
using System.Globalization;
using System.Reflection;

public class FuzzEndToEndTest
{
    string [] arg;
    Dictionary <int,int> dictionaryOracleExit = new Dictionary<int, int>();
    Dictionary <string[], string[]> dictionaryOracleCommands = new Dictionary<string[], string[]> ();
    List <string[]> argumentsObserve = new List<string[]>();
    List <string[]> argumentsProposal = new List<string[]>();
    List <string[]> argumentsComment = new List<string[]>();
    List <string> addedIdentifierObs = new List<string>();
    List <string> addedIdentifierCom = new List<string>();
    List <string> addedIdentifierPro = new List<string>();
    long startObsId;
    long endObsID;
        
    private string[] generateArguments (string command) {
        var rootCommand = new RootCommand();
        if (command.Equals("observe"))
        {
            arg = ["observe", generateString(), generateString()];
            argumentsObserve.Add(arg);
            dictionaryOracleCommands = getParsedValues(arg, dictionaryOracleCommands);
        } else if (command.Equals("proposal"))
        {
            string id = "1"; // these tests only write proposals to observation with id 1
            addedIdentifierPro.Add(id);
            var taxonId = generateTaxonID();
            arg = ["proposal",taxonId,id];
            addedIdentifierPro.Add(taxonId);
            argumentsProposal.Add(arg);
            dictionaryOracleCommands = getParsedValues(arg,dictionaryOracleCommands);
        } else
        {
            string id = "1"; //this test only comments to observation with id 1
            addedIdentifierCom.Add(id);
            var randString = generateString();
            arg = ["comment",randString, id];
            addedIdentifierCom.Add(randString);
            argumentsComment.Add(arg);
            dictionaryOracleCommands = getParsedValues(arg,dictionaryOracleCommands);
        }

        return arg;
    }

    public Dictionary<string[],string[]> getParsedValues (string [] arg, Dictionary<string[], string[]> dictionaryOracleCommands)
    {
        var Argument1 = new Argument<string>(arg[1]);
        var Argument2 = new Argument<string>(arg[2]);
        var ArgumentCommand = new Argument<string>(arg[0]);
        var command = new Command(arg[0]);
        string seq = arg[0] + " " + arg[1] + " " + arg[2];
        var result = command.Parse(seq);
        string[] actual = [result.GetValue(ArgumentCommand), result.GetValue(Argument1), result.GetValue(Argument2)];
        dictionaryOracleCommands.Add(arg,actual);
        return dictionaryOracleCommands;
    }

    [Fact]
    public async Task ObservationsRand ()
    {
        for (int i = 0; i < 50 ; i++)
        {
            //Arrange
            var path = "../../../../../src/Bison.CLI/bison_observe_cli_db.csv";
            CSVDatabase<Observations> database = CSVDatabase<Observations>.getInstance();
            List<Observations> observerecords = database.ReadObservation(path).ToList<Observations>();
            startObsId = database.GetNumberOfLinesInAFile(path);
            
            arg = generateArguments("observe");


            //Act
            int result = await Program.Main(arg);
            
            //Arrange
            endObsID = database.GetNumberOfLinesInAFile(path);
            for (long j = startObsId; j < endObsID; j++)
            {
                addedIdentifierObs.Add(j.ToString());
            }
            
            //Act
            Task <int> exitcode = new Task<int>(() => 0);
            exitcode.Start();
            await exitcode;

            //Assert
            Assert.Equal(0, result);
            
            //Cleanup
            //editObsFile(path);
        }
    }

    [Fact]
    public async Task ProposalsRand ()
    {
        for (int i = 0; i < 100 ; i++)
        {
            //Arrange
            arg = generateArguments("proposal");

            //Act
            int result = await Program.Main(arg);

            Task <int> exitcode = new Task<int>(() => 0);
            exitcode.Start();
            await exitcode;

            //Assert
            Assert.Equal(0, result);
            
            //Cleanup
            var path = "../../../../../src/Bison.CLI/bison_proposal_cli_db.csv";
            //editProFile(path);
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

            //Assert
            Assert.Equal(0, result);
            
            //Cleanup
            var path = "../../../../../src/Bison.CLI/bison_comment_cli_db.csv";
            //editComFile(path);
        }
    }

     [Fact]
    public void testOracle ()
    {
        foreach (KeyValuePair<string [], string []> entry in dictionaryOracleCommands)
        {
            Assert.True(entry.Key == entry.Value);
        }
    } 

    //https://www.geeksforgeeks.org/c-sharp/c-sharp-randomly-generating-strings/
    public static string generateString ()
    {
        Random rand = new Random();

        int stringlen = rand.Next(1, 10);
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
    
    public static string generateTaxonID()
    {
        /*Random rand = new Random();
        Treebuilder tb = Treebuilder.getInstance();
        HashSet<string> set = tb.getIdToTaxonDictionary.Keys;
        string element = set.ElementAtOrDefault(rand.Next(set.Count()));
        return element;*/
        //okay so the above is commented-out because this class for some reason doesnt recognize TreeBuilder but this is how it would work to give random taxons.
        return "MSTSNM:Arter:eeb1f9f3-f785-ea11-aa77-501ac539d1ea"; //just until we have the actual method
    }
    
    /*
    These methods are based on https://stackoverflow.com/questions/64036022/c-sharp-how-to-delete-certain-rows-from-a-csv-file-and-save-it-as-a-new-csv-usin#64047685
    The purpose is to give a file and it will delete the inserted test data from the file
    */
    private void editObsFile(string path)
    {   
        CSVDatabase<Observations> database = CSVDatabase<Observations>.getInstance();
        List<Observations> observerecords = database.ReadObservation(path).ToList<Observations>();
        
        for (int i = 0; i < observerecords.Count(); i++)
        {
            var id = observerecords[i].ID.ToString();
            if (addedIdentifierObs.Contains(id))
            { //this doesnt account for duplicates of observations
                observerecords.RemoveAt(i);
            }
        }
        
       
            foreach (var obs in observerecords)
            {
                database.Store(obs, path, obs.Location);
            }
            /*csvWriter.NextRecord();
            csvWriter.WriteHeader<Observations>();
            csvWriter.NextRecord();
            foreach (var record in observerecords)
            {
                csvWriter.WriteRecord(record);
                csvWriter.NextRecord();
            }*/
            //this isn't writing the records correctly into the file so instead use the store method from csvdatabase
        
    }
    
    private void editProFile(string path)
    {
        CSVDatabase<Proposal> database = CSVDatabase<Proposal>.getInstance();
        List<Proposal> proposalrecords = database.ReadProposals(path, 1).ToList<Proposal>();
        
        for (int i = 0; i < proposalrecords.Count(); i++)
        {
            var taxonId = proposalrecords[i].TaxonId;
            var id = proposalrecords[i].ObservationId.ToString();
            if (addedIdentifierPro.Contains(id) && addedIdentifierPro.Contains(taxonId))
            { //we cannot remove proposals only based on their obs id since it would remove more than the ones from the test
                proposalrecords.RemoveAt(i);
            }
        }     
                
        using (var writer = new StreamWriter(path))
        using (var csvWriter = new CsvWriter(writer, CultureInfo.InvariantCulture))
        {
            csvWriter.WriteRecords(proposalrecords);
            //this isn't writing the records correctly into the file so instead use the store method from csvdatabase
        }
    }
    
    private void editComFile(string path)
    {
        CSVDatabase<Comment> database = CSVDatabase<Comment>.getInstance();
        List<Comment> commentrecords = database.ReadDiscussion(path, 1).ToList<Comment>();
        for (int i = 0; i < commentrecords.Count(); i++)
        {
            var comment = commentrecords[i].Observation;
            var id = commentrecords[i].ObservationId.ToString();
            if (addedIdentifierCom.Contains(id) && addedIdentifierCom.Contains(comment))
            { //we cannot remove comments on simply their obs id since that would remove more than the ones from this test
                commentrecords.RemoveAt(i);
            }
        }
                    
        using (var writer = new StreamWriter(path))
        using (var csvWriter = new CsvWriter(writer, CultureInfo.InvariantCulture))
        {
            csvWriter.WriteRecords(commentrecords);
            //this isn't writing the records correctly into the file so instead use the store method from csvdatabase
        }
    }
}