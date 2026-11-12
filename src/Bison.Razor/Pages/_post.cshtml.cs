using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Bison.Razor.ObservationService;
using Bison.Razor.TaxonService;

namespace Bison.Razor.Pages;

public class PostModel : PageModel
{
    private readonly IObservationService _service;
    public List<ObservationViewModel> Observations { get; set; }

    public PostModel(IObservationService service)
    {
        _service = service;
    }
    /*

    public async Task<ActionResult> OnSet([FromQuery] string text)
    {
        
       
    }*/
}
