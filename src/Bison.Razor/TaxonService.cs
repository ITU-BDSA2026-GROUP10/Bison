namespace Bison.Razor.TaxonService;
using Bison.Razor.ObservationService; 
using DataModel;

public interface ITaxonService
{
    public List<Observation> GetObservationsFromTaxonId (string id, List<Observation> observations);
}

public class TaxonService : ITaxonService
{
    private IPostRepository postRepository;

    public TaxonService(IPostRepository postRepository)
    {
        this.postRepository = postRepository;
    }

    public List<Observation> GetObservationsFromTaxonId(string id, List<Observation> observations){
        List <Observation> filteredObservations = new List<Observation>();
        foreach(var ob in observations){
            if(ob.Taxon.dwc_TaxonID.Equals(id)) {
                filteredObservations.Add(ob);
            } else {
                foreach (var child in ob.Taxon.Children){
                    if(child.dwc_TaxonID.Equals(id)){
                        filteredObservations.Add(ob);
                    }
                }
            }
        }
        return filteredObservations;
    }

    /*public List<Observation> GetObservationsFromTaxonIdRECURSION(Taxon taxon, List<Observation> observations)
    {
        if(observations.Count() == 0)
        {
            return new List<Observation>();
        } else
        {
            GetObservationsFromTaxonIdRECURSION1(taxon, )
            return null;
        }
    }

    public Observation GetObservationsFromTaxonIdRECURSION1(Taxon taxon, List<Observation> observations)
    {
        return null;
    }*/
}
