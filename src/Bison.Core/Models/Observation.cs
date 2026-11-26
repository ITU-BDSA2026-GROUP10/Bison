using System.Diagnostics;
//using Microsoft.AspNetCore.Authentication;

namespace Bison.Core.Models;

public class Observation : Post
{
    public Taxon? Taxon {get; set;}

    public List<Comment>? Comments {get; set;}

    public List<Proposal>? Proposals {get; set;}

      public int Likes {get; set;} = 0;

    public Taxon getTaxon()
    {
        return Taxon;
    }
}