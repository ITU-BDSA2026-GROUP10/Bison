using System;
using System.IO;
using System.Reflection.Metadata;
using CsvHelper;
using System.Globalization;
using SimpleDB;
using System.CommandLine;
using System.CommandLine.Parsing;
using System.Net.Http.Json;
using System.Net.Http.Headers;
using System.Text.Json;


public class Program
{
    // https://learn.microsoft.com/en-us/dotnet/api/system.net.http.httpclient?view=net-10.0

    static async Task<int> Main(string[] args)
    {

        RootCommand rootCommand = new RootCommand();

        HttpClient client = new HttpClient();
        
        client.DefaultRequestHeaders.Accept.Clear();
        client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
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
        Argument<string> observationIdArgument = new Argument<string>("observationID");
        Argument<string> discussionArgument = new Argument<string>("observationId");
        Argument<string> locationArgument = new Argument<string>("location");
        Argument<string> locationArgumentForLocationCommand = new Argument<string>("location");

        observeCommand.Arguments.Add(observationArgument);
        observeCommand.Arguments.Add(locationArgument);
        commentCommand.Arguments.Add(commentArgument);
        commentCommand.Arguments.Add(observationIdArgument);
        discussionCommand.Arguments.Add(discussionArgument);
        locationCommand.Arguments.Add(locationArgumentForLocationCommand);

        ReadCommands(readCommand, client);
        DiscussionCommands(discussionCommand, client, discussionArgument);
        ObserveCommands(observeCommand, client, observationArgument, locationArgument);
        CommentCommands(commentCommand, client, commentArgument, observationIdArgument);
<<<<<<< HEAD
        LocationCommands(locationCommand, client, locationArgumentForLocationCommand);
=======

        /*observeCommand.SetAction(async parseResult =>

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

            /*try
            {
                string observation = parseResult.GetValue(observationArgument);
                string location = parseResult.GetValue(locationArgument);
                var response = await client.GetFromJsonAsync<IEnumerable<Observations>>($"http://localhost:5252/observation?observation={observation=observation}");
            } catch (HttpRequestException e)
            {
                Console.WriteLine("\nException Caught!");
                Console.WriteLine("Message: {0} ", e.Message);
            }


        });*/

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


           /* commentCommand.SetAction(parseResult =>
        {
            SetCommentAction(parseResult, commentArgument);
        }
        );*/

>>>>>>> week4

        ParseResult parseResult = rootCommand.Parse(args);
        try
        {
            foreach (var unmatchedToken in parseResult.UnmatchedTokens)
            {
                throw new ArgumentException("This action does not exist, please try a valid action");
            }
            // parseResult.Invoke();
        }
        catch (ArgumentException e)
        {
            Console.WriteLine(e.Message);
        }

        return await parseResult.InvokeAsync();
    }

    private static async void ObserveCommands(Command observeCommand, HttpClient client, Argument<string> observationArgument, Argument<string> locationArgument)
    {
        observeCommand.SetAction(async parseResult =>
       {
           try
           {
               string observation = parseResult.GetValue(observationArgument);
               string location = parseResult.GetValue(locationArgument);
               
               var response = await client.PostAsJsonAsync($"http://localhost:5252/observation?observation={observation}&location={location}", new {observation, location});
           } catch (HttpRequestException e)
           {
               Console.WriteLine("\nException Caught!");
               Console.WriteLine("Message: {0} ", e.Message);
           }
       });

    }

    private static async void ReadCommands(Command readCommand, HttpClient client)
    {
     readCommand.SetAction(async parseResult =>
        {
            try {
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
    
    private static async void DiscussionCommands(Command discussionCommand, HttpClient client, Argument<string> discussionArgument)
    {
        discussionCommand.SetAction(async parseResult =>
        { 
            try {
                long ObservationId = long.Parse(parseResult.GetValue(discussionArgument));
                var response = await client.GetFromJsonAsync<IEnumerable<Comment>>($"http://localhost:5252/comments?observationId={ObservationId}");
                if (response != null)
                    UserInterface.printDiscussion(ObservationId, response);
            } catch (HttpRequestException e)
            {
                Console.WriteLine("\nException Caught!");
                Console.WriteLine("Message: {0} ", e.Message);
            }
        });
    }
   
    public static async void CommentCommands(Command commentCommand, HttpClient client, Argument<string> commentArgument, Argument<string> observationIdArgument)
    {
        commentCommand.SetAction(async parseResult =>
        {
            try
            {
                if (parseResult.GetValue(commentArgument) != null && !parseResult.GetValue(commentArgument).Equals(""))
                {
                    string comment = parseResult.GetValue(commentArgument);
                    string id = parseResult.GetValue(observationIdArgument);

                    var response = await client.PostAsJsonAsync($"http://localhost:5252/comment?comment={comment}&id={id}", new {comment, id});
                }
                else
                {
                    throw new ArgumentException("Comment cannot be null or empty string, please enter a valid observation.");
                }
            }
            catch (HttpRequestException e)
            {
                Console.WriteLine("\nException Caught!");
                Console.WriteLine("Message: {0} ", e.Message);
            }
            catch (ArgumentException e)
            {
                Console.WriteLine(e.Message);
            }
        });
    }

    public static async void LocationCommands(Command locationCommand, HttpClient client, Argument<string> locationArgument2)
    {
        locationCommand.SetAction(async parseResult =>
        {
            try 
            {
                string location = parseResult.GetValue(locationArgument2);

                if(location != null && !location.Equals(""))
                {
                    var response = await client.GetFromJsonAsync<IEnumerable<Observations>>($"http://localhost:5252/location?location={location}");
                    if (response != null)
                        UserInterface.printObservationsByLocation(location, response);
                } else
                {
                    throw new ArgumentException("Location cannot be null or empty string, please enter a valid location.");
                }
            }
            catch (HttpRequestException e)
            {
                Console.WriteLine("\nException Caught!");
                Console.WriteLine("Message: {0} ", e.Message);
            } 
            catch (ArgumentException e)
            {
                Console.WriteLine(e.Message);
            }
        }
        );
    }
}
<<<<<<< HEAD
=======


 
>>>>>>> week4
