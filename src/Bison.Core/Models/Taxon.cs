namespace Bison.Core.Models;
public class Taxon
{
    public int TaxonId {get; set;}
    public string dwc_TaxonID {get; set;} = "";
    public string VernacularName {get; set;} = "";
    public Taxon? Parent {get; set;}
    public ICollection<Taxon>? Children {get; set;}

    public bool isSubTaxon(Taxon taxon)
    {
        return taxon.Children.Contains(this);
    }
}