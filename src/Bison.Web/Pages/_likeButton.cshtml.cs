using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Bison.Infrastructure.Services;

namespace Bison.Web.Pages;

public class LikeModel : PageModel
{
    private readonly IObservationService _service;

    public LikeModel(IObservationService service)
    {
        _service = service;
    }
}