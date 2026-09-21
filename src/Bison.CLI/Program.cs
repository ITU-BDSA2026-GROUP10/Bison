using System;
using System.IO;
using System.Reflection.Metadata;
using CsvHelper;
using System.Globalization;
using SimpleDB;
using System.CommandLine;
using System.CommandLine.Parsing;
using System.Net.Http.Json;

public class Program
{   
    // https://learn.microsoft.com/en-us/dotnet/api/system.net.http.httpclient?view=net-10.0

    static readonly HttpClient client = new HttpClient();
    client.BaseAddress = new Uri("http://localhost:5252");
    static async Task<int> Main (string[] args)
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
        
        observeCommand.Arguments.Add(observationArgument);
        commentCommand.Arguments.Add(commentArgument);
        discussionCommand.Arguments.Add(discussionArgument);
        locationCommand.Arguments.Add(locationArgument);

        readCommand.SetAction(async parseResult =>
        {
            /*CSVDatabase<Observations> csvDatabase = CSVDatabase<Observations>.getInstance(); // Skal ændres?
            IEnumerable<Observations> enumerator = csvDatabase.ReadObservation("bison_observe_cli_db.csv"); // Skal ændres? */
            try {
                // using HttpResponseMessage response = await client.GetFromJsonAsync<List<T>>("http://localhost:5252/observations");
                var response = await client.GetFromJsonAsync<IEnumerable<Observations>>("http://localhost:5252/observations");
                if (response != null)
                    UserInterface.printObservations(response);
            } catch (HttpRequestException e)
            {
                Console.WriteLine("\nException Caught!");
                Console.WriteLine("Message: {0} ", e.Message);
            }
        });

        discussionCommand.SetAction(async parseResult =>
        { //this is just what happens when a user tries to do the read command - so this should be changed to list comments
            /*CSVDatabase<Comment> csvDatabase = CSVDatabase<Comment>.getInstance(); //Skal ændres?
            IEnumerable<Comment> enumerator = csvDatabase.ReadDiscussion("bison_comment_cli_db.csv",discussionArgumentLong); // Skal ændres?
            UserInterface.printDiscussion(discussionArgumentLong, enumerator);*/
            try {
                long ObservationId = long.Parse(parseResult.GetValue(discussionArgument));
                Console.WriteLine(ObservationId);
                var response = await client.GetFromJsonAsync<IEnumerable<Comment>>($"http://localhost:5252/comments?observationId={ObservationId = ObservationId}");
                if (response != null)
                    UserInterface.printDiscussion(ObservationId, response);
            } catch (HttpRequestException e)
            {
                Console.WriteLine("\nException Caught!");
                Console.WriteLine("Message: {0} ", e.Message);
            }
        });

        observeCommand.SetAction(parseResult =>
        {
            try {
                if(parseResult.GetValue(observationArgument) != null && !parseResult.GetValue(observationArgument).Equals(""))
                {
                    string observation = parseResult.GetValue(observationArgument);
                    Observations observation1 = new Observations(null, observation, 0,0);
                    if(observation != null)
                    {
                        CSVDatabase<Observations> csvDatabase = CSVDatabase<Observations>.getInstance(); // Skal ændres?
                        csvDatabase.Store(observation1); // Skal ændres?
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
    
        return await parseResult.InvokeAsync();
    }

    private async void readCommands(Command readCommand)
    {
            try {
                // using HttpResponseMessage response = await client.GetFromJsonAsync<List<T>>("http://localhost:5252/observations");
                var response = await client.GetFromJsonAsync<IEnumerable<Observations>>("http://localhost:5252/observations");
                if (response != null)
                    UserInterface.printObservations(response);
            } catch (HttpRequestException e)
            {
                Console.WriteLine("\nException Caught!");
                Console.WriteLine("Message: {0} ", e.Message);
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
                        CSVDatabase<String> csvDatabase = CSVDatabase<String>.getInstance(); //Skal ændres
                        csvDatabase.StoreComment(actualComment,"bison_observe_cli_db.csv", observationId); // Skal ændres?
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
 