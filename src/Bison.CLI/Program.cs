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
        Command commentCommand = new ("comment");
        Command discussionCommand = new ("discussion");

        rootCommand.Add(readCommand);
        rootCommand.Add(observeCommand);
        rootCommand.Add(commentCommand);
        rootCommand.Add(discussionCommand);
    
        Argument<string> observationArgument = new Argument<string>("observation");
        Argument<string> commentArgument = new Argument<string>("comment");
        Argument<string> discussionArgument = new Argument<string> ("observationId");
        
        observeCommand.Arguments.Add(observationArgument);
        commentCommand.Arguments.Add(commentArgument);
        discussionCommand.Arguments.Add(discussionArgument);

        readCommand.SetAction(parseResult =>
        {
            CSVDatabase<Observations> csvDatabase = new CSVDatabase<Observations>();
            IEnumerable<Observations> enumerator = csvDatabase.Read("bison_observe_cli_db.csv");
            Console.WriteLine("inde i program, hej!");
            UserInterface.printObservations(enumerator);
        });

        discussionCommand.SetAction(parseResult =>
        { //this is just what happens when a user tries to do the read command - so this should be changed to list comments
            CSVDatabase<Comment> csvDatabase = new CSVDatabase<Comment>();
            IEnumerable<Comment> enumerator = csvDatabase.Read("bison_comment_cli_db.csv");
            Console.WriteLine("inde i discussion command");
            long discussionArgumentLong = long.Parse(parseResult.GetValue(discussionArgument));
            UserInterface.printDiscussion(discussionArgumentLong, enumerator);
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
                        csvDatabase.Store(observation,"bison_observe_cli_db.csv"); 
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

        commentCommand.SetAction(parseResult =>
        {
            SetCommentAction(parseResult, commentArgument);
        }
        );


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
    public static void SetCommentAction(ParseResult parseResult, Argument <string> commentArgument)
    {
        try {
                if(parseResult.GetValue(commentArgument) != null && !parseResult.GetValue(commentArgument).Equals(""))
                {
                    string comment = parseResult.GetValue(commentArgument);
                    if(comment != null)
                    {
                        long observationId = 0;
                        int endOfId = 0;
                        char[] charArray = comment.ToCharArray();
                        for(int i = 0; i<charArray.Length; i++)
                        {
                            char ch = charArray[i];
                            if (ch.Equals(',')) //finds the end of the observation id
                            {
                                endOfId = i;
                            }
                        }
                        observationId = long.Parse(comment.Substring(0, endOfId)); //the id of the observation that this is a comment for
                        string actualComment = comment.Substring(endOfId+2);
                        CSVDatabase<String> csvDatabase = new CSVDatabase<String>();
                        csvDatabase.StoreComment(actualComment,"bison_comment_cli_db.csv", "bison_observe_cli_db.csv", observationId);
                    }
                } else
                {
                    throw new ArgumentException("Comment cannot be null or empty string, please enter a valid observation.");
                }
            }
            catch (ArgumentException e)
            {
                Console.WriteLine(e.Message);
            }
    }
}
 