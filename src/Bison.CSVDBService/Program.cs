using System.Xml;
using SimpleDB;

var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();
IDatabaseRepository<Observations> databaseObs = CSVDatabase<Observations>.getInstance();
IDatabaseRepository<Comment> databaseCom = CSVDatabase<Comment>.getInstance();
IDatabaseRepository<Proposal> databasePro = CSVDatabase<Proposal>.getInstance();
CSVDatabase<Taxon> database = CSVDatabase<Taxon>.getInstance();
database.ReadTaxon();

app.MapGet("/observations", () =>
{
    return databaseObs.ReadObservation("../Bison.CLI/bison_observe_cli_db.csv");
});

app.MapGet("/comments", (long observationId) =>
{
    return databaseCom.ReadDiscussion("../Bison.CLI/bison_comment_cli_db.csv",observationId);
});

app.MapGet("/location", (string location) =>
{
    return databaseObs.ReadObservation("../Bison.CLI/bison_observe_cli_db.csv");
}
);

app.MapGet("/proposals", (long observationId) =>
{
    return databasePro.ReadProposals("../Bison.CLI/bison_proposal_cli_db.csv", observationId);
});

app.MapPost("/proposal", (string taxonId, long observationId) => {
    string proposalpath = "../Bison.CLI/bison_proposal_cli_db.csv";
    string observationPath = "../Bison.CLI/bison_observe_cli_db.csv";
    long localTime = DateTimeOffset.Now.ToUnixTimeSeconds() + 7200; //+7200 is to make the time match our time-zone
    string userName = Environment.UserName;
    Proposal pro = new Proposal(userName, taxonId, localTime, observationId);
    databasePro.StoreProposal(pro, proposalpath, observationPath, observationId);
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

app.Run();
