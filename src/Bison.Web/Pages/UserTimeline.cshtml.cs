using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Bison.Infrastructure.Services;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Bison.Core.Models;

namespace Bison.Web.Pages;

public class UserTimelineModel : PageModel
{
    private readonly IObservationService _service;
    // This is a comment
    public List<ObservationViewModel> Observations { get; set; }

    public int CurrentPage { get; set; }
    public string Text {get; set;}

    public string Author {get; set;}

    public UserTimelineModel(IObservationService service)
    {
        _service = service;
    }
    
    public async Task<IActionResult> OnPost([FromForm] string observation, [FromRoute] string author)
    {
        //save to database
        //start by making a observation and then call repos method addPost()
        if (User.Identity.IsAuthenticated && User.Identity.Name == author)
        {
            var auth = await _service.SetAuthor(author, author); //supposed to be name and then email
            _service.AddObservationPost(auth, observation);
            return RedirectToPage("/UserTimeline", new { authorId = auth.AuthorId });
            //evt send back which page to redirect to
        } else
        {
            return Forbid();
        }
    }

    public async Task<ActionResult> OnGet([FromRoute] string author, [FromQuery] int page)
    {
        CurrentPage = ObservationService.ValidPageNumber(page);
        Task <List<ObservationViewModel>> obsTask = _service.GetObservationsFromAuthor(author,page);
        Observations = await obsTask;
        return Page();
    }
}
