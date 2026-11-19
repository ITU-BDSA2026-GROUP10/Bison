namespace Bison.Core.DTOs;

public class ProposalDTO
{ 
    public int ObservationID {get; set;}
    public int AuthorID {get; set;}
    public string AuthorName {get; set;} = "";
    public string Text {get; set;} = "";
    public string Taxon {get; set;} = "";
    public string TimeStamp {get; set;}
}