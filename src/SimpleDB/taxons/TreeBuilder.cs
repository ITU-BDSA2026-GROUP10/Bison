
namespace taxons;
class TreeBuilder {

    Tree tree = new Tree();
    
void build(IEnumerable<Taxon> taxons)
    {

        foreach (var taxon in taxons) 
        {
            //maybe this is where we go through the list of records and then assign parents and children.
        
            if (taxon.TaxonRank == "order")
            {
                tree.SetRoot(taxon);
                tree.addChild(null, taxon); //parent should be null
            } else
            {
            tree.addChild(taxon.ParentNameUsageID, taxon);
            } 
        }

        
    }
}