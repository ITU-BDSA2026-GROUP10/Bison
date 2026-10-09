using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Bison.Razor.Pages;

public class ObIdModel : PageModel
{
    private readonly IObservationService _service;
    public List<ObservationViewModel> Observations { get; set; }

    public ObIdModel(IObservationService service)
    {
        _service = service;
    }

    public ActionResult OnGet([FromQuery] int id, [FromQuery] int page)
    {
        Observations = _service.GetObservationFromID(id, page);
        return Page();
    }
}
