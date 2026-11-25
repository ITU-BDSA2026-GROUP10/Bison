using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Bison.Infrastructure.Services;
using Microsoft.EntityFrameworkCore.Infrastructure.Internal;
using Bison.Core.Models;
using System.Transactions;

namespace Bison.Web.Pages;

public class PublicModel : PageModel
{
    private readonly IObservationService _service;
    public List<ObservationViewModel> Observations { get; set; }
    
    //public List<CommentViewModel> Comments { get; set; }

    public int CurrentPage { get; set; }
    public string Text {get; set;}

    public string Author {get; set;}

    public int counter {get; set;}

    public PublicModel(IObservationService service)
    {
        _service = service;
    }

    public async Task<ActionResult> OnPostLike([FromHeader] int id)
    {
        Console.WriteLine("on post like id: " + id);
        var auth = await _service.GetAuthor(User.Identity.Name, User.Identity.Name); //supposed to be name and then email, but both are email right now?
        counter++;
        Console.WriteLine("Pressed like button this many times: " + counter);
        await _service.AddFavorites(auth, id);
        return RedirectToPage("Public");
    }
    public async Task<ActionResult> OnGet([FromQuery] int page)
    {
        CurrentPage = ObservationService.ValidPageNumber(page);
        Observations = await _service.GetObservations(page);
        return Page();
    }
    
}

