//this snippet is unchanged* from slides
module {:extern "src.DataModel"} Classes{
    class {:extern} Taxon {
        function method {:extern} isSubTaxon(ancestor: Taxon): bool
    }

    class {:extern} Observation {
        function method {:extern} getTaxon(): Taxon 
    }
}
//end snippet

module Filter{
    import opened Classes
    //See explaination on non-ghost and ghost methods and functions: https://github.com/hath995/dafny-lang/blob/master/docs/HowToFAQ/Errors-Resolution.md
    function method FilterBy(root: Taxon, obs: seq<Observation>): seq<Observation>
    
    //now every thing we that gets returned comes from the input and matches
    //==> means "implies"
    ensures forall ob: Observation :: ob in FilterBy(root, obs) ==>
    ob in obs && ob.getTaxon().isSubTaxon(root)
    
    //and the inputs that match are returned
    ensures forall ob: Observation :: ob in obs && ob.getTaxon().isSubTaxon(root) ==>
    ob in FilterBy(root, obs)
    
    //now cardinality check
    ensures |FilterBy(root, obs)| <= |obs|
    
    decreases |obs| //'decreases' is neccesary in dafny because of the need for a program to terminate and this works with both loops and recursion
    //Therefore you can insert 'decreases' and this makes sure the obs sequence strictly decreases and that it can't decrease forever
    //for more see: https://dafny.org/dafny/OnlineTutorial/Termination
    { //this is where the function body actually is:
        if |obs| == 0 then 
            [] //this is the step the function will hopefully result in
        else 
            assert obs == [obs[0]] + obs[1..]; //in dafny 1.. is the rest of the sequence
            //the assert is for dafny to know which stm to prove each time is always true
            
            if obs[0].getTaxon().isSubTaxon(root)
                then [obs[0]] + FilterBy(root, obs[1..]) //the recursion
            else 
                FilterBy(root, obs[1..])
    }
}

//now the action:
module FilterWrapped {
    import opened Filter 
    import opened Classes
    
    method filterObservations (root: Taxon, obs: array<Observation>)
    returns (result: array<Observation>)
    
    ensures fresh(result) 
    //'fresh(result)' returns a boolean value that is true 
    //if the objects denoted by expression 'result' were all freshly allocated 
    //since the time of entry to the enclosing method - https://dafny.org/dafny/DafnyRef/DafnyRef#sec-fresh-expression 
    
    //now everything that is returned comes from input observations and matches
    ensures forall ob: Observation :: ob in result[..] ==>
    ob in obs[..] && ob.getTaxon().isSubTaxon(root)
    
    //here we have closeness because we ensure the opposite as well
    ensures forall ob: Observation :: ob in obs[..] && ob.getTaxon().isSubTaxon(root) ==>
    ob in result[..]
    
    { //the method body
        var filtered := FilterBy(root, obs[..]);
        var size := |filtered|;
        result := new Observation[size](i requires 0 <= i < size => filtered[i]); //this is another way to write seq(size, i => P) which is a sequence display expression
        //this allows us to define the sequence to be constructed as length |filtered| and each value is evaluated based on the condition i == P see https://dafny.org/dafny/DafnyRef/DafnyRef#sec-seq-comprehension
        
        assert result[..] == filtered;
    }
}

//http://dafny.org/latest/Dafny-cheat-sheet.pdf