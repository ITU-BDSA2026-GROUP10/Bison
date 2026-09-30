
using System.Xml;


namespace taxons;
class TreeBuilder {

    Tree tree = new Tree();
    
void build(IEnumerable<Taxon> taxons)
    {

        foreach (var taxon in taxons) 
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
        }
        
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