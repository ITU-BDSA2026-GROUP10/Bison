//https://www.geeksforgeeks.org/dsa/introduction-to-tree-data-structure/

class Node
{
    public Taxon taxon;
    
    public Node supertaxon;
    public List<Node> subtaxons;
    public Node (Taxon tax)
        {
            taxon = tax;
            subtaxons = new List<Node>();
        }
}

class Tree {
    Node root;
    List<Node> taxonNodes;
    
public Tree()
    {
        taxonNodes = new List<Node>();
    }


public void SetRoot(Node taxon)
    {
        if (root == null)
        {
            root = taxon;
        }
    }

public void addChild(Node Parent, Node taxon)
{
    Parent.subtaxons.Add(taxon);
    taxonNodes.Add(taxon);
}

public List<Node> getNodes(){
    return taxonNodes;
}

}