
using System.Xml;
using Microsoft.VisualBasic;


namespace taxons;
public class TreeBuilder {

    Tree tree = new Tree();
    Dictionary<string, Taxon> idToTaxon = new Dictionary<string, Taxon>();
    Dictionary<string, Taxon> vernacularNameToTaxon = new Dictionary<string, Taxon>();
    
public void build(IEnumerable<Taxon> taxons)
    {
        
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
        
        /*foreach (var taxon in taxons) 
        {
            //maybe this is where we go through the list of records and then assign parents and children.
            Node n = new Node(taxon);
            if (taxon.taxonRank == "order")
            {
                tree.SetRoot(n);
                //tree.addChild(null, taxon); //parent should be null
            } else
            {
                //tree.addChild(n.parent, n);
            } 
        }*/
        
    }
    
public void mapTaxonPairings(IEnumerable<Taxon> taxons)
    {
        foreach (var taxon in taxons)
        {
            if(!idToTaxon.ContainsKey(taxon.taxonID)) idToTaxon.Add(taxon.taxonID, taxon);
            if(!vernacularNameToTaxon.ContainsKey(taxon.vernacularName)) vernacularNameToTaxon.Add(taxon.vernacularName, taxon);
        }

        /*foreach (var taxon in idToTaxon)
        {
            Console.WriteLine(taxon.Key + ": " + taxon.Value);
        }*/
    }
void treeBuild(IEnumerable<Node> taxons)
    {
        foreach (var Node in taxons)
        {
            tree.SetRoot(Node); //because it will only assign when null and the first is always the root
            //tree.addChild(Node.previous, Node);
        }
    }
}