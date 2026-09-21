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

    static async Task<int> Main(string[] args)
    {

        RootCommand rootCommand = new RootCommand();

        HttpClient client = new HttpClient();
        client.BaseAddress = new Uri("http://localhost:5252");

        Command readCommand = new("read");
        Command observeCommand = new("observe");
        Command commentCommand = new("comment");
        Command discussionCommand = new("discussion");
        Command locationCommand = new("location");

        rootCommand.Add(readCommand);
        rootCommand.Add(observeCommand);
        rootCommand.Add(commentCommand);
        rootCommand.Add(discussionCommand);
        rootCommand.Add(locationCommand);

        Argument<string> observationArgument = new Argument<string>("observation");
        Argument<string> commentArgument = new Argument<string>("comment");
        Argument<string> discussionArgument = new Argument<string>("observationId");
        Argument<string> locationArgument = new Argument<string>("location");
        Argument<string> locationArgument2 = new Argument<string>("location");

        observeCommand.Arguments.Add(observationArgument);
        commentCommand.Arguments.Add(commentArgument);
        discussionCommand.Arguments.Add(discussionArgument);
        locationCommand.Arguments.Add(locationArgument2);

        ReadCommands(readCommand, client);
       

        discussionCommand.SetAction(async parseResult =>
        { 
            try {
                long ObservationId = long.Parse(parseResult.GetValue(discussionArgument));
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

            /*if (parseResult.GetValue(observationArgument) != null && !parseResult.GetValue(observationArgument).Equals(""))
                try
                {
                    string observation = parseResult.GetValue(observationArgument);
                    string location = parseResult.GetValue(locationArgument);

                    if ((observation != null && !observation.Equals("")) && (location != null && !location.Equals("")))
                    {
                        CSVDatabase<string> csvDatabase = CSVDatabase<string>.getInstance();
                        csvDatabase.Store(observation, "bison_observe_cli_db.csv", location);
                    }
                    else
                    {
                        throw new ArgumentException("Observation or location cannot be null or empty string, please enter valid input.");
                    }
                }
                catch (ArgumentException e)
                {
                    Console.WriteLine(e.Message);
                }*/

            try
            {
                string observation = parseResult.GetValue(observationArgument);
                string location = parseResult.GetValue(locationArgument);
                //var response = await client.GetFromJsonAsync<IEnumerable<Observations>>($"http://localhost:5252/observation?observation={observation=observation}");
            } catch (HttpRequestException e)
            {
                Console.WriteLine("\nException Caught!");
                Console.WriteLine("Message: {0} ", e.Message);
            }


        });

        locationCommand.SetAction(parseResult =>
        {
            try
            {
                string location = parseResult.GetValue(locationArgument2);

                if ((location != null && !location.Equals("")))
                {
                    CSVDatabase<Observations> csvDatabase = CSVDatabase<Observations>.getInstance();
                    IEnumerable<Observations> enumerator = csvDatabase.ReadObservation("bison_observe_cli_db.csv");


                    UserInterface.printObservationsByLocation(location, enumerator);

                }
                else
                {
                    throw new ArgumentException("Location cannot be null or empty string, please enter a valid location.");
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

    private static async void ReadCommands(Command readCommand, HttpClient client)
    {
     readCommand.SetAction(async parseResult =>
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
        });   
    }
    public static void SetLocationAction(ParseResult parseResult, Argument<string> locationArgument2)
     {
        try 
            {
                string location = parseResult.GetValue(locationArgument2);

                if((location != null && !location.Equals("")))
                {
                    CSVDatabase<Observations> csvDatabase = CSVDatabase<Observations>.getInstance();
                    IEnumerable<Observations> enumerator = csvDatabase.ReadObservation("bison_observe_cli_db.csv");
                    

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
    public static void SetCommentAction(ParseResult parseResult, Argument<string> commentArgument)
    {
        try
        {
            if (parseResult.GetValue(commentArgument) != null && !parseResult.GetValue(commentArgument).Equals(""))
            {
                string comment = parseResult.GetValue(commentArgument);
                if (comment != null)
                {
                    long observationId = 0;
                    int endOfId = 0;
                    char[] charArray = comment.ToCharArray();
                    for (int i = 0; i < charArray.Length; i++)
                    {
                        char ch = charArray[i];
                        if (ch.Equals(',')) //finds the end of the observation id
                        {
                            endOfId = i;
                        }
                    }
                    observationId = long.Parse(comment.Substring(0, endOfId)); //the id of the observation that this is a comment for
                    string actualComment = comment.Substring(endOfId + 2);
                    CSVDatabase<String> csvDatabase = CSVDatabase<String>.getInstance(); //Skal ændres
                    csvDatabase.StoreComment(actualComment, "bison_observe_cli_db.csv", observationId); // Skal ændres?
                }
            }
            else
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

 