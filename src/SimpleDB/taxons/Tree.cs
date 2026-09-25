//https://www.geeksforgeeks.org/dsa/introduction-to-tree-data-structure/

class Tree {
    Taxon root;
class Node
{
    public string taxon;
    public List<Node> subtaxons;
        
}

void SetRoot(Taxon taxon)
    {
        if (root == null)
        {
            root = taxon;
        }
    }

}