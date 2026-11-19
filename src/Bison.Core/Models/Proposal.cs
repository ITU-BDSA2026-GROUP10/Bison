namespace Bison.Core.Models;

public class Proposal : Post
{
    public Observation Observation {get; set;}
    public Taxon Taxon {get; set;}
}