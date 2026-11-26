using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Bison.Infrastructure.Services;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Bison.Core.Models;
using Microsoft.AspNetCore.Components.Forms;
using System.ComponentModel.DataAnnotations;

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

    //[BindProperty]
    //public InputModel Input {get; set;}

    public UserTimelineModel(IObservationService service)
    {
        _service = service;
    }
    
    public async Task<IActionResult> OnPost([FromForm] string observation, [FromForm] IFormFile photo, [FromRoute] string author)
    {
        Text = observation;
        Photo = photo;
        
        Console.WriteLine("Observation: " + Text);
        Console.WriteLine("Photo: " + Photo);
        
        if (User.Identity.IsAuthenticated && User.Identity.Name == author)
        {   
            var auth = await _service.SetAuthor(author, author); //supposed to be name and then email
            Console.WriteLine("Dette er min observation: " + Text);
            var id = _service.AddObservationPost(auth, Text);
            if(Photo != null && Photo.Length > 0)
            {
                Console.WriteLine("Photo length is greater than 0");
                var path = Path.Combine("wwwroot/uploads", id.ToString());
                path = path + ".png";
                await using var stream = new FileStream(path, FileMode.Create);
                await Photo.CopyToAsync(stream);
            }
            Console.WriteLine(author + " har lavet denne observation " + Text);
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

 /*public class InputModel
    {
        [Display (Name = "Description")]
        public string Text { get; set; } = "";
        /*[Display (Name = "Taxon")]
        public int TaxonId { get; set; }*/
        //public IFormFile? Photo { get; set; } // optional
    //}