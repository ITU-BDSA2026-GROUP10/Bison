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
        Command locationCommand = new ("location");

        rootCommand.Add(readCommand);
        rootCommand.Add(observeCommand);
        rootCommand.Add(commentCommand);
        rootCommand.Add(discussionCommand);
        rootCommand.Add(locationCommand);
    
        Argument<string> observationArgument = new Argument<string>("observation");
        Argument<string> commentArgument = new Argument<string>("comment");
        Argument<string> discussionArgument = new Argument<string> ("observationId");
        Argument<string> locationArgument = new Argument<string> ("location");
        Argument<string> locationArgument2 = new Argument<string> ("location");
        
        observeCommand.Arguments.Add(observationArgument);
        observeCommand.Arguments.Add(locationArgument);
        commentCommand.Arguments.Add(commentArgument);
        discussionCommand.Arguments.Add(discussionArgument);
        locationCommand.Arguments.Add(locationArgument2);

        readCommand.SetAction(parseResult =>
        {
            CSVDatabase<Observations> csvDatabase = CSVDatabase<Observations>.getInstance();
            IEnumerable<Observations> enumerator = csvDatabase.ReadObservation("bison_observe_cli_db.csv");
            UserInterface.printObservations(enumerator);
        });

        discussionCommand.SetAction(parseResult =>
        { //this is just what happens when a user tries to do the read command - so this should be changed to list comments
            CSVDatabase<Comment> csvDatabase = CSVDatabase<Comment>.getInstance();
            long discussionArgumentLong = long.Parse(parseResult.GetValue(discussionArgument));
            IEnumerable<Comment> enumerator = csvDatabase.ReadDiscussion("bison_comment_cli_db.csv",discussionArgumentLong);
            UserInterface.printDiscussion(discussionArgumentLong, enumerator);
        });

        observeCommand.SetAction(parseResult =>
        {
            try 
            {
                string observation = parseResult.GetValue(observationArgument);
                string location = parseResult.GetValue(locationArgument);
                
                if((observation != null && !observation.Equals("")) && (location != null && !location.Equals("")))
                {
                    CSVDatabase<string> csvDatabase = CSVDatabase<string>.getInstance();
                    csvDatabase.Store(observation,"bison_observe_cli_db.csv",location);
                } else
                {
                    throw new ArgumentException("Observation or location cannot be null or empty string, please enter valid input.");
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

        locationCommand.SetAction(parseResult =>
        {
            try 
            {
                string location = parseResult.GetValue(locationArgument2);

                if((location != null && !location.Equals("")))
                {
                    CSVDatabase<Observation> csvDatabase = CSVDatabase<Observation>.getInstance();
                    IEnumerable<Observation> enumerator = csvDatabase.ReadObservation("bison_observe_cli_db.csv");
                    

                    UserInterface.printObservationsByLocation(location, enumerator);

                } else
                {
                    throw new ArgumentException("Location cannot be null or empty string, please enter a valid location.");
                }
    
            
            } catch (ArgumentException e)
            {
                Console.WriteLine(e.Message);
            }
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
                        CSVDatabase<String> csvDatabase = CSVDatabase<String>.getInstance();
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
    public static void SetLocationAction()
    {
        
    }
}
 