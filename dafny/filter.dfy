class {:extern "Taxon"} Taxon {
    function {:extern} isSubTaxon(ancestor: Taxon): bool
}

class {:extern "Observation"} Observation {
    function {:extern} getTaxon(): Taxon 
}

function FilterBy(root: Taxon, obs: seq<Observation>): seq<Observation> {
    var i := 0;
    //if |obs| == 0 then [] else [];
    var result := [];
    while i < |obs|{
        if obs[i].getTaxon() == root then result + [root]
        else if 

        i := i + 1;
    }
}