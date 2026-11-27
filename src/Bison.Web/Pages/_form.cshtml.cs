using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Bison.Infrastructure.Services;
using System.ComponentModel.DataAnnotations;

namespace Bison.Web.Pages;

public class FormModel : PageModel
{
    private readonly IObservationService _service;
    public List<ObservationViewModel> Observations { get; set; }
    
    [BindProperty]
    public InputModel Input {get; set;} = new();
    public FormModel(IObservationService service)
    {
        _service = service;
    }

    public class InputModel
    {
        [Display (Name = "Description")]
        public string Text { get; set; } = "";
        /*[Display (Name = "Taxon")]
        public int TaxonId { get; set; }*/
        public IFormFile? Photo { get; set; } // optional
    }
/*
    public async Task<ActionResult> OnPost(string description)
    {
        //do something with text
        return await RedirectToPage("/{obs?}"); //after a post
       
    }*/
}
