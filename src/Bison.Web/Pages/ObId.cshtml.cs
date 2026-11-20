using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Bison.Infrastructure.Services;
using Bison.Core.Models;

namespace Bison.Web.Pages;

public class ObIdModel : PageModel
{
    private readonly IObservationService _service;
    public List<ObservationViewModel> Observations { get; set; }

    public int CurrentPage { get; set; }

    public ObIdModel(IObservationService service)
    {
        _service = service;
    }

    public async Task<IActionResult> OnPostComment([FromForm] string comment, [FromQuery] int id)
    {
        //save to database
        //start by making a observation and then call repos method addPost()
        if (User.Identity.IsAuthenticated)
        {
            var auth = await _service.SetAuthor(User.Identity.Name, User.Identity.Name); //supposed to be name and then email, but both are email right now?
            Console.WriteLine("Dette er min comment: " + comment);
            await _service.AddCommentPost(auth, comment, id);
            Console.WriteLine(User.Identity.Name + " har lavet denne comment " + comment);
            return RedirectToPage("/ObId", new { id = id });
            //evt send back which page to redirect to
        }
        else
        {
            return Forbid();
        }
    }

    public async Task<IActionResult> OnPostProposal([FromForm] string proposal, [FromQuery] int id, [FromForm] int taxonId)
    {
        //save to database
        //start by making a observation and then call repos method addPost()
        if (User.Identity.IsAuthenticated)
        {
            var auth = await _service.SetAuthor(User.Identity.Name, User.Identity.Name); //supposed to be name and then email, but both are email right now?
            Console.WriteLine("Dette er min proposal: " + proposal);
            await _service.AddProposalPost(auth, proposal, id, taxonId);
            Console.WriteLine(User.Identity.Name + " har lavet denne proposal " + proposal);
            return RedirectToPage("/ObId", new { id = id });
            //evt send back which page to redirect to
        }
        else
        {
            return Forbid();
        }
    }

    public async Task<ActionResult> OnGet([FromQuery] int id, [FromQuery] int page)
    {
        CurrentPage = ObservationService.ValidPageNumber(page);
        Observations = await _service.GetObservationFromID(id, page);
        return Page();
    }


}
