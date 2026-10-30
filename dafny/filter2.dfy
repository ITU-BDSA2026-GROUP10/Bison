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

//http://dafny.org/latest/Dafny-cheat-sheet.pdf