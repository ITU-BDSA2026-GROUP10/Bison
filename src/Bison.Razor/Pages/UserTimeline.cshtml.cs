using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Bison.Razor.ObservationService;
using Bison.Razor.TaxonService;
using System.Threading.Tasks;

namespace Bison.Razor.Pages;

public class UserTimelineModel : PageModel
{
    private readonly IObservationService _service;
    public List<ObservationViewModel> Observations { get; set; }

    public string Text {get; set;}

    public string Author {get; set;}

    public UserTimelineModel(IObservationService service)
    {
        _service = service;
    }
    
    public void OnSet([FromQuery] string text, [FromRoute] string author, [FromRoute] string email)
    {
        //save to database
        //start by making a observation and then call repos method addPost()
       var auth = _service.SetAuthor(author, email);
       _service.AddObservationPost(auth, text);
        
        //evt send back which page to redirect to
    }

    public async Task<ActionResult> OnGet([FromRoute] string author, [FromQuery] int page)
    {
        Task <List<ObservationViewModel>> obsTask = _service.GetObservationsFromAuthor(author,page);
        Observations = await obsTask;
        return Page();
    }
}
