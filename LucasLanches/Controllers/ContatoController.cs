using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace LucasLanches.Controllers
{
    [Authorize]
    public class ContatoController : Controller
    {
        public IActionResult Index()
        {
            return View();
           
        }
    }
}
