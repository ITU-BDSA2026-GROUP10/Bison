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
    
    public async Task<ActionResult> OnGet([FromRoute] string author, [FromQuery] int page)
    {
        var auth = await _service.GetAuthor(User.Identity.Name, User.Identity.Name); 
        CurrentPage = ObservationService.ValidPageNumber(page);
        Observations = await _service.GetFavoritesFromAuthor(auth);

        /*Task <List<ObservationViewModel>> obsTask = await _service.GetFavoritesFromAuthor(auth);
        Observations =  obsTask;*/
        
        return Page();
    }
}
