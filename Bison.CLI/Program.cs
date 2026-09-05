using System;
using System.IO;
using System.Reflection.Metadata;
using CsvHelper;
using System.Globalization;
using SimpleDB;
using System.CommandLine;
using System.CommandLine.Parsing;

public class Program
{

    static void Main (string[] args)
    {
      
        RootCommand rootCommand = new RootCommand();
    
        Command readCommand = new ("read"); 
        Command observeCommand = new ("observe");

        rootCommand.Add(readCommand);
        rootCommand.Add(observeCommand);
    
        Argument<string> observationArgument = new Argument<string>("observation");
        
        observeCommand.Arguments.Add(observationArgument);

        readCommand.SetAction(parseResult =>
        {
            CSVDatabase<Cheep> csvDatabase = new CSVDatabase<Cheep>();
            IEnumerable<Cheep> enumerator = csvDatabase.Read();
            UserInterface.printObservations(enumerator);
        });

        observeCommand.SetAction(parseResult =>
        {
            string observation = parseResult.GetValue(observationArgument);
            CSVDatabase<string> csvDatabase = new CSVDatabase<string>();
            csvDatabase.Store(observation);  
        });


        ParseResult parseResult = rootCommand.Parse(args);
        parseResult.Invoke();




       /*try
        {  
            if(args[0] == "read")
            {
                CSVDatabase<Cheep> csvDatabase = new CSVDatabase<Cheep>();
                IEnumerable<Cheep> enumerator = csvDatabase.Read();
                UserInterface.printObservations(enumerator);

            } else if(args[0] == "observe")
            {
                CSVDatabase<string> csvDatabase = new CSVDatabase<string>();
                csvDatabase.Store(args[1]);                
            }

        } catch (IOException e)
        {
            Console.WriteLine("The file could not be read:");
            Console.WriteLine(e.Message);
        } finally
        {
            
        }*/   
    }
}
 