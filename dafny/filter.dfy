class {:extern} Taxon {
    var {:extern ""} issubTaxon: bool
    function {:extern} isSubTaxon(ancestor: Taxon): bool
    ensure issubTaxon == bool
}

class {:extern} Observation {
    function {:extern} getTaxon(): Taxon
}

function FilterBy(root: Taxon, obs: seq<Observation>): seq<Observation>
//http://dafny.org/latest/Dafny-cheat-sheet.pdf