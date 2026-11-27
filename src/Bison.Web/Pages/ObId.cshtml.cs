using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Bison.Infrastructure.Services;
using Bison.Core.Models;
using Microsoft.EntityFrameworkCore.Query.SqlExpressions;
using Bison.Core.DTOs;

namespace Bison.Web.Pages;

public class ObIdModel : PageModel
{
    private readonly IObservationService _service;
    private readonly ITaxonService _taxonService;
    public List<ObservationViewModel> Observations { get; set; }
    public List<TaxonDTO> Taxons {get; set;}

    public int CurrentPage { get; set; }

    public ObIdModel(IObservationService service, ITaxonService Taxonservice)
    {
        _service = service;
        _taxonService = Taxonservice;
    }

    public async Task<IActionResult> OnPostComment([FromForm] string comment, [FromQuery] int id)
    {  
        //save to database
        //start by making a observation and then call repos method addPost()
        if (User.Identity.IsAuthenticated)
        {
            var auth = await _service.GetAuthor(User.Identity.Name, User.Identity.Name); //supposed to be name and then email, but both are email right now?
            await _service.AddCommentPost(auth, comment, id);
            return RedirectToPage("/ObId", new { id = id });
            //evt send back which page to redirect to
        }
        else
        {
            return Forbid();
        }
    }

    public async Task<ActionResult> OnPostProposal([FromForm] string proposal, [FromQuery] int id, [FromForm] string taxonId)
    {
        Console.WriteLine("%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%% my taxon id is: " + taxonId);
        Console.WriteLine("%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%% my obid is: " + id);
        Console.WriteLine("%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%% my proposal is: " + proposal);
        //proposal = "I suggest this bird is actually: "; //Just for now until you can post text with your proposal
        //save to database
        //start by making a observation and then call repos method addPost()
        if (User.Identity.IsAuthenticated)
        {
            var auth = await _service.GetAuthor(User.Identity.Name, User.Identity.Name); //supposed to be name and then email, but both are email right now?
            await _service.AddProposalPost(auth, proposal, id, taxonId);
            return RedirectToPage("/ObId", new { id = id });
            //evt send back which page to redirect to
        }
        else
        {
            return Forbid();
        }
    }

    /*public async Task<ActionResult> OnGet([FromQuery] int id, [FromQuery] int page)
    {
        Observations = await _service.GetObservationFromID(id, page);
        return Page();
    } */

    public async Task<ActionResult> OnGetAsync ([FromQuery] int id, [FromQuery] int page)
    {
        CurrentPage = ObservationService.ValidPageNumber(page);
        Observations = await _service.GetObservationFromID(id, page);
        Taxons = await _taxonService.GetAllTaxons();
        return Page();
        //return RedirectToPage("/ObId", new { id = id });
    }

}
