/*using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Bison.Infrastructure.Services;

namespace Bison.Web.Pages;

public class CommentModel : PageModel
{
    private readonly IObservationService _service;
    public List<ObservationViewModel> Observations { get; set; }
    //public List<CommentViewModel> Comments { get; set; }

    public CommentModel(IObservationService service)
    {
        _service = service;
    }
    
    public async Task<ActionResult> OnGet([FromQuery] int i)
    {
        return Pages();
    }
}*/
