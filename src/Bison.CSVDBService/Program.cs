using System.Xml;
using SimpleDB;

var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();
CSVDatabase<Cheep> database = new SimpleDB.CSVDatabase<Cheep>();
/*app.MapGet("/observations", () => new Observation("signe","Heron at DR Byen",1788161296,3));
app.MapPost("/observations", (Observation observation) => database.Store(observation,"bison_observe_cli_db.csv")); */

app.MapGet("/observations", () =>
{
    return database.Read("bison_observe_cli_db.csv");
});

app.MapGet("/comments", () =>
{
    return database.Read("bison_comment_cli_db.csv");
});

app.MapPost("/observation", (Observation observation) =>
{
    database.Store(observation,"bison_observe_cli_db.csv");
});

app.MapPost("/comment", (Comment comment) =>
{
    database.StoreComment(comment,"bison_comment_cli_db.csv",comment.Observation,comment.ObservationId);
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