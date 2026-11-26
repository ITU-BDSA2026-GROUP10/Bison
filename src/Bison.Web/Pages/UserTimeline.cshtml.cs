using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Bison.Infrastructure.Services;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Bison.Core.Models;
using Microsoft.AspNetCore.Components.Forms;

namespace Bison.Web.Pages;

public class UserTimelineModel : PageModel
{
    private readonly IObservationService _service;
    // This is a comment
    public List<ObservationViewModel> Observations { get; set; }

    public int CurrentPage { get; set; }
    public string Text {get; set;}

    public string Author {get; set;}
    public IFormFile? Photo {get; set;}
    public class InputModel
    {
        [Display (Name = "Description")]
        public string Text { get; set; } = "";
        /*[Display (Name = "Taxon")]
        public int TaxonId { get; set; }*/
        public IFormFile? Photo { get; set; } // optional
    }

    public UserTimelineModel(IObservationService service)
    {
        _service = service;
    }
    
    public async Task<IActionResult> OnPost([FromRoute] string author)
    {
        //save to database
        //start by making a observation and then call repos method addPost()
        if (User.Identity.IsAuthenticated && User.Identity.Name == author)
        {
            if(Input.Photo is {Length: > 0})
            {
                var path = Path.Combine("wwwroot/uploads", Input.Photo.FileName);
                await using var stream = new FileStream(path, FileMode.Create);
                await photo.CopyToAsync(stream);
            }
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
        CurrentPage = ObservationService.ValidPageNumber(page);
        Task <List<ObservationViewModel>> obsTask = _service.GetObservationsFromAuthor(author,page);
        Observations = await obsTask;
        return Page();
    }
}
