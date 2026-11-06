namespace Bison.Razor.TaxonService;

using System.ComponentModel.DataAnnotations;
using Bison.Razor.ObservationService; 
using DataModel;
using DTO;
//using Filter;

public interface ITaxonService
{
    public Task<List<ObservationViewModel>> GetObservationsFromTaxonID (string id, int page);
    public void filteredObservations(List<Observation> observations);
}

public class TaxonService : ITaxonService
{
    private IPostRepository postRepository;

    public TaxonService(IPostRepository postRepository)
    {
        this.postRepository = postRepository;
    }

    public List<ObservationDTO> Filter(string id, List<ObservationDTO> observations){
        List <ObservationDTO> filteredObservations = new List<ObservationDTO>();
        foreach(var ob in observations){
            if(ob.Taxon != null)
            {
                if(ob.Taxon.dwc_TaxonID.Equals(id)) {
                    filteredObservations.Add(ob);
                } else {
                    if(ob.Taxon.Children != null)
                    {
                        foreach (var child in ob.Taxon.Children){
                            if(child.dwc_TaxonID.Equals(id)){
                                filteredObservations.Add(ob);
                            }
                        }
                    }
                    
                }
            }
            
        }
        return filteredObservations;
    }


    public async Task<List<ObservationViewModel>> GetObservationsFromTaxonID(string id, int page)
    {

        page = ValidPageNumber(page);

        var observationDTOs = await postRepository.ReadPosts();

        List<ObservationDTO> filteredObservations = Filter(id, observationDTOs);
        List<ObservationViewModel> observations = new List<ObservationViewModel>();
        //var observationDTOs = await postRepository.ReadObservationsWithTaxonId(id, page);
        foreach (var DTO in filteredObservations)
        {
            observations.Add(new ObservationViewModel(DTO.AuthorName, DTO.Text, DTO.TimeStamp.ToString()));
        }
        
        return observations; 
    }

    public static int ValidPageNumber(int page) {
        if (page <= 0)
        {
            page = 1;
        }

        return page;
    }
    public void filteredObservations(List<Observation> observations)
    {
        //Filter.__default.FilterWrapped(observations);

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
