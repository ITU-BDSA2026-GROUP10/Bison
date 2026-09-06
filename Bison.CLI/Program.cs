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
            try {
                if(parseResult.GetValue(observationArgument) != null && !parseResult.GetValue(observationArgument).Equals(""))
                {
                    string observation = parseResult.GetValue(observationArgument);
                    if(observation != null)
                    {
                        CSVDatabase<string> csvDatabase = new CSVDatabase<string>();
                        csvDatabase.Store(observation); 
                    }
                } else
                {
                    throw new ArgumentException("Observation cannot be null or empty string, please enter a valid observation.");
                }
            }
            catch (ArgumentException e)
            {
                Console.WriteLine(e.Message);
            }
        });


        ParseResult parseResult = rootCommand.Parse(args);
        try
        {
            foreach (var unmatchedToken in parseResult.UnmatchedTokens)
            {
                throw new ArgumentException("This action does not exist, please try a valid action");
            }
            parseResult.Invoke();
        }
        catch (ArgumentException e)
        {
            Console.WriteLine(e.Message);
        }
        


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
 