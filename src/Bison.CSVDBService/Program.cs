using System.Xml;
using SimpleDB;

var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();
CSVDatabase<Observations> databaseObs = CSVDatabase<Observations>.getInstance();
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
    
    string path = "../Bison.CLI/bison_observe_cli_db.csv";
    long localTime = DateTimeOffset.Now.ToUnixTimeSeconds() + 7200; //+7200 is to make the time match our time-zone
    long id = databaseObs.GetNumberOfLinesInAFile(path);
    string userName = Environment.UserName;

    Observations obs = new Observations(userName, observation, localTime, id, location);
    //string path = "../Bison.CLI/bison_observe_cli_db.csv";
    databaseObs.Store(obs, path, location);
});

app.MapPost("/comment", (string comment, string id) =>
{
    string observationPath = "../Bison.CLI/bison_observe_cli_db.csv";
    string commentPath = "../Bison.CLI/bison_comment_cli_db.csv";
    
    long idAsLong = long.Parse(id);
    long localTime = DateTimeOffset.Now.ToUnixTimeSeconds() + 7200; //+7200 is to make the time match our time-zone
    string userName = Environment.UserName;

    Comment com = new Comment(userName, comment, localTime, idAsLong);

    //databaseCom.StoreComment(comment,observationPath, commentPath, idAsLong);
    //databaseCom.StoreComment(comment,comment.Observation,comment.ObservationId);
    
    databaseCom.StoreComment(com, observationPath, commentPath, idAsLong);
}); 

app.MapPost("/hello", () =>
{
    return "hello";
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


