/*using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Bison.Infrastructure.Services;
using Bison.Core.Models;

namespace Bison.Web.Pages;

public class CommentModel : PageModel
{
    private readonly IObservationService _service;
    public Observation observation { get; set; }
    public string Text {get; set;}
    public int CurrentPage { get; set; }

    public string Author {get; set;}

    public CommentModel(IObservationService service)
    {
        _service = service;
    }
    
public async Task<IActionResult> OnPostComment([FromForm] string comment, [FromRoute] string author, [FromRoute] int obId)
    {
        //save to database
        //start by making a observation and then call repos method addPost()
        if (User.Identity.IsAuthenticated)
        {
            var auth = await _service.SetAuthor(author, author); //supposed to be name and then email
            Console.WriteLine("Dette er min comment: " + observation);
            await _service.AddCommentPost(auth, comment, obId);
            Console.WriteLine(author + " har lavet denne comment " + comment);
            return RedirectToPage("/ObId", new { id = obId });
            //evt send back which page to redirect to
        } else
        {
            return Forbid();
        }
    }

    public async Task<ActionResult> OnGet([FromRoute] string author, [FromQuery] int page)
    {
        CurrentPage = ObservationService.ValidPageNumber(page);
        //Task<List<Comment>> comTask = _service.GetComments(author, page);
        //Comments = await comTask;
        return Page();
    }
}*/
