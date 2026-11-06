using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Bison.Razor.ObservationService;
using Bison.Razor.TaxonService;

namespace Bison.Razor.Pages;

public class FilterModel : PageModel
{
    private readonly ITaxonService _service;
    public List<ObservationViewModel> Observations { get; set; }

    public FilterModel(ITaxonService service)
    {
        _service = service;
    }

    public async Task<ActionResult> OnGet([FromQuery] string id, [FromQuery] int page)
    {
        //Observations = await _service.GetObservationsDTO(page);

        Observations = await _service.GetObservationsFromTaxonID(id, page);
        /*Observations = await _service.GetObservationsFromTaxonId();
        Observations = await _service.GetObservationFromID(id, page);*/
        return Page();
    }
}
