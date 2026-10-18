namespace DTO;
public class ObservationDTO
{ //TODO: DOUBLE CHECK THIS 
    public int PostID {get; set;}
    public int AuthorID {get; set;}
    public string AuthorName {get; set;}
    //public int AuthorID {get; set;}
    public string Text {get; set;} = "";
    public int Timestamp {get; set;}
}