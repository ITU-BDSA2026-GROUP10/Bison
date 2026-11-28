using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Bison.Infrastructure.Services;
using Microsoft.EntityFrameworkCore.Infrastructure.Internal;
using Bison.Core.Models;
using System.Transactions;
using Bison.Core.DTOs;

namespace Bison.Web.Pages;

public class PublicModel : PageModel
{
    private readonly IObservationService _service;
    private readonly ITaxonService _taxonService;
    public List<ObservationViewModel> Observations { get; set; }
    public List<TaxonDTO> Taxons {get; set;}
    
    //public List<CommentViewModel> Comments { get; set; }

    public int CurrentPage { get; set; }
    public string Text {get; set;}

    public string Author {get; set;}

    public int counter {get; set;}

    public PublicModel(IObservationService service,  ITaxonService Taxonservice)
    {
        _service = service;
        _taxonService = Taxonservice;
    }

    public async Task<ActionResult> OnPostFilter([FromForm] string TaxonId)
    {
        Console.WriteLine("--------------------Filter bliver kaldt");
        return RedirectToPage("Filter", new {id = TaxonId});
    }
    public async Task<ActionResult> OnPostLike([FromQuery] int id)
    {
        var auth = await _service.GetAuthor(User.Identity.Name, User.Identity.Name); //supposed to be name and then email, but both are email right now?
        counter++;
        await _service.AddFavorites(auth, id);
        return RedirectToPage("Public");
    }
    public async Task<ActionResult> OnGet([FromQuery] int page)
    {
        CurrentPage = ObservationService.ValidPageNumber(page);
        Observations = await _service.GetObservations(page);
        Taxons = await _taxonService.GetAllTaxons();
        return Page();
    }
    
}

