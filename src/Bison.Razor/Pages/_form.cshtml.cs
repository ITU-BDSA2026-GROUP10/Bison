using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Bison.Infrastructure.Services;

namespace Bison.Razor.Pages;

public class FormModel : PageModel
{
    private readonly IObservationService _service;
    public List<ObservationViewModel> Observations { get; set; }

    public FormModel(IObservationService service)
    {
        _service = service;
    }
/*
    public async Task<ActionResult> OnPost(string description)
    {
        //do something with text
        return await RedirectToPage("/{obs?}"); //after a post
       
    }*/
}
