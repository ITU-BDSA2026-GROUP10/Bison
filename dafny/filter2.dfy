class {:extern "Taxon"} Taxon {
    constructor() {
    }
    function {:extern} isSubTaxon(ancestor: Taxon): bool
    
}

class {:extern "Observation"} Observation {
    var {:extern "Taxon"} taxon: Taxon 
    constructor() {taxon := new Taxon();}
    function {:extern "getTaxon"} getTaxon(): Taxon 
       reads this
        ensures taxon is Taxon
        
}

function FilterBy(root: Taxon, obs: seq<Observation>): seq<Observation> {
    var i := 0;
    while i < |obs|
        if root in obs then Add(root);
        i := i + 1;
}

//second try
function FilterBy(root: Taxon, obs: seq<Observation>): seq<Observation> {
    //if |obs| == 0 then [] else [];
    var result := [];
    forall ob: Observation | obs {
        ob.isSubTaxon(root);
    }
        
        //obs isSubTaxon() .... FilterBy(getTaxon())
        
      
    //if obs[i].getTaxon() == root then result + [root]
    //else if 
    
    //assert obs == [obs[0] .. obs[1..]];
}

//http://dafny.org/latest/Dafny-cheat-sheet.pdf