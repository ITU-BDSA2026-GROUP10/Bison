
using System.Xml;
using Microsoft.VisualBasic;


namespace taxons;
sealed public class TreeBuilder {

    private static readonly TreeBuilder instance = new TreeBuilder();

    public Dictionary<string, Taxon> idToTaxon = new Dictionary<string, Taxon>();
    Dictionary<string, Taxon> vernacularNameToTaxon = new Dictionary<string, Taxon>();

    public static TreeBuilder getInstance()
    {
        return instance;
    }
    
public void build(IEnumerable<Taxon> taxons)
    {
        Console.WriteLine("building tree!!");
        foreach (var parent in taxons)
        {
            foreach(var child in taxons)
            {
                if(child.parentNameUsageID == parent.taxonID)
                {
                    parent.subtaxons.Add(child);
                }
            }
        }
        
    }
    
public void mapTaxonPairings(IEnumerable<Taxon> taxons)
    {
        foreach (var taxon in taxons)
        {
            if(taxon.taxonID is not null && !idToTaxon.ContainsKey(taxon.taxonID)) {
                idToTaxon.Add(taxon.taxonID, taxon);
            }
            if (taxon.vernacularName is not null && !vernacularNameToTaxon.ContainsKey(taxon.vernacularName))
            {
                vernacularNameToTaxon.Add(taxon.vernacularName, taxon);
            } 
        }
    }

    public Dictionary<string, Taxon> getIdToTaxonDictionary()
    {  
        return idToTaxon;
    }
}
