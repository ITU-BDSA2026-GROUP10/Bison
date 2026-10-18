namespace DataModel;
public class Taxon
{
    public string VernacularName {get; set;}
    public int TaxonId {get; set;}
    public string dwc_TaxonID {get; set;}
    public ICollection<Taxon> Children {get; set;}
    public Taxon? Parent {get; set;}
}