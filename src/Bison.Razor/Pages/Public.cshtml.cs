using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Bison.Razor.ObservationService;
using Bison.Razor.TaxonService;
using Microsoft.EntityFrameworkCore.Infrastructure.Internal;
using DataModel;

namespace Bison.Razor.Pages;

public class PublicModel : PageModel
{
    private readonly IObservationService _service;
    public List<ObservationViewModel> Observations { get; set; }
    public int CurrentPage { get; set; }

    public string Text {get; set;}

    public string Author {get; set;}

    public PublicModel(IObservationService service)
    {
        _service = service;
    }

    public async Task<ActionResult> OnGet([FromQuery] int page)
    {
        CurrentPage = ObservationService.ObservationService.ValidPageNumber(page);
        Observations = await _service.GetObservations(page);
        return Page();
    }

}

