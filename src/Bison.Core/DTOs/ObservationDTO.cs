using Bison.Core.Models;

namespace Bison.Core.DTOs;
public class ObservationDTO
{ //TODO: DOUBLE CHECK THIS 
    public int PostID { get; set; }
    public int AuthorID { get; set; }
    public string AuthorName { get; set; }
    //public int AuthorID {get; set;}
    public string Text { get; set; } = "";
    public string TimeStamp { get; set; }
    //public string? PhotoPath {get; set;}
    public Taxon? Taxon { get; set; }
}