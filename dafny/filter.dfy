module {:extern "src.DataModel"} Classes{
    class {:extern} Taxon {
        function {:extern} isSubTaxon(ancestor: Taxon): bool
    }

    class {:extern} Observation {
        function {:extern} getTaxon(): Taxon 
    }
}

module Filter {
    import opened Classes
    //Brug både rekursion OG forall
    function FilterBy(root: Taxon, obs: seq<Observation>): seq<Observation> 
        //==> means "implies"
        
        //ensures soundness
        ensures forall o: Observation :: o in FilterBy(root, obs) ==>
            o in obs && o.getTaxon().isSubTaxon(root)

        //ensures completeness
        ensures forall o: Observation :: o.getTaxon().isSubTaxon(root) ==>
            o in FilterBy(root, obs)
        
        ensures |FilterBy(root, obs)| <= |obs|

        {
            // this is the actual like function (code is made by chat):
            if |obs| == 0 then
                []
            else 
                assert obs == [obs[0]] + obs[1..];
                var first := obs[0];
                var rest := FilterBy(root, obs[1..]);

                if first.getTaxon().isSubTaxon(root) then
                    [first] + rest
                else
                    rest
            
                
        //assert obs == obs[0] + obs[1..];
        }
}
