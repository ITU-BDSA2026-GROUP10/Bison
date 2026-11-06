using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Bison.Razor.ObservationService;
using Bison.Razor.TaxonService;

namespace Bison.Razor.Pages;

public class PublicModel : PageModel
{
    private readonly IObservationService _service;
    public List<ObservationViewModel> Observations { get; set; }

    public PublicModel(IObservationService service)
    {
        _service = service;
    }

    public async Task<ActionResult> OnGet([FromQuery] int page)
    {
        Observations = await _service.GetObservations(page);
        return Page();
    }
}
