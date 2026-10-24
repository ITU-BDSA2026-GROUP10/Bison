namespace DTO;
public class ProposalDTO
{ //TODO: DOUBLE CHECK THIS 
    public int ObservationID {get; set;}
    public int AuthorID {get; set;}
    public string AuthorName {get; set;} = "";
    public string Text {get; set;} = "";
    public string Taxon {get; set;} = "";
    public string TimeStamp {get; set;}
}