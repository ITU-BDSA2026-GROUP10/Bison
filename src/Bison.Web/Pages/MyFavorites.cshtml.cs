using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Bison.Infrastructure.Services;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Bison.Core.Models;

namespace Bison.Web.Pages;

public class MyLikesModel : PageModel
{
    private readonly IObservationService _service;
    // This is a comment
    public List<ObservationViewModel> Observations { get; set; }

    public int CurrentPage { get; set; }
    public string Text {get; set;}

    public string Author {get; set;}

    public MyLikesModel(IObservationService service)
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
            Console.WriteLine("Dette er min observation: " + observation);
            _service.AddObservationPost(auth, observation);
            Console.WriteLine(author + " har lavet denne observation " + observation);
            return RedirectToPage("/UserTimeline", new { authorId = auth.AuthorId });
            //evt send back which page to redirect to
        } else
        {
            return Forbid();
        }
    }

    public async Task<ActionResult> OnGet([FromRoute] string author, [FromQuery] int page)
    {
        var auth = await _service.SetAuthor(author, author); 
        CurrentPage = ObservationService.ValidPageNumber(page);
        Observations = await _service.GetFavoritesFromAuthor(auth);

        /*Task <List<ObservationViewModel>> obsTask = await _service.GetFavoritesFromAuthor(auth);
        Observations =  obsTask;*/
        
        return Page();
    }
}
