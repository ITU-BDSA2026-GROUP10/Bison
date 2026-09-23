using System.Xml;
using SimpleDB;

var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();
CSVDatabase<string> databaseObs = CSVDatabase<string>.getInstance();
CSVDatabase<Comment> databaseCom = CSVDatabase<Comment>.getInstance();
/*app.MapGet("/observations", () => new Observation("signe","Heron at DR Byen",1788161296,3));
app.MapPost("/observations", (Observation observation) => database.Store(observation,"bison_observe_cli_db.csv")); */

app.MapGet("/observations", () =>
{
    return databaseObs.ReadObservation("../Bison.CLI/bison_observe_cli_db.csv");
});

app.MapGet("/comments", (long observationId) =>
{
    return databaseCom.ReadDiscussion("../Bison.CLI/bison_comment_cli_db.csv",observationId);
});

app.MapPost("/observation", (string observation,  string location) =>
{
    Console.WriteLine("trying to post!!!");
    

    /*string path = "../Bison.CLI/bison_observe_cli_db.csv";
    long localTime = DateTimeOffset.Now.ToUnixTimeSeconds() + 7200; //+7200 is to make the time match our time-zone
    long id = GetNumberOfLinesInAFile(path);
    string userName = Environment.UserName;

    Observations obs = new Observations(userName, observation, localTime, id, location);*/
    string path = "../Bison.CLI/bison_observe_cli_db.csv";
    databaseObs.Store(observation, path, location);
});

app.MapPost("/comment", (Comment comment) =>
{
    databaseCom.StoreComment(comment, comment.Observation,comment.ObservationId);
}); 

app.Run();

/*static Observation getObservation(string Author, string Message, long Timestamp, long ID)
{
return new Observation(Author,Message,Timestamp,ID);
}

static XmlComment getComment(string Author, string Message, long Timestamp, long ID)
{
return new Observation(Author,Message,Timestamp,ID);
} */


