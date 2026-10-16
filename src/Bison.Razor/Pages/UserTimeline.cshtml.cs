using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Bison.Razor.BisonService;
using System.Threading.Tasks;

namespace Bison.Razor.Pages;

public class UserTimelineModel : PageModel
{
    private readonly IObservationService _service;
    public List<ObservationViewModel> Observations { get; set; }

    public UserTimelineModel(IObservationService service)
    {
        _service = service;
    }

    public async Task<ActionResult> OnGet([FromRoute] string author, [FromQuery] int page)
    {
        Task <List<ObservationViewModel>> obsTask = _service.GetObservationsFromAuthor(author,page);
        Observations = await obsTask;
        return Page();
    }
}
