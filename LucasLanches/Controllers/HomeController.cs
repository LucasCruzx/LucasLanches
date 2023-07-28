using LucasLanches.Models;
using LucasLanches.Repositories.Interfaces;
using LucasLanches.ViewModels;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace LucasLanches.Controllers
{
    public class HomeController : Controller
    {
       private readonly ILancheRepository _lancheRepository;

        public HomeController(ILancheRepository lancheRepository)
        {
            _lancheRepository = lancheRepository;
        }

        public IActionResult Index()
        {
            var homeViewModel = new HomeViewModel
            {
                LanchesPreferidos = _lancheRepository.LanchesPreferidos,

            };

            return View(homeViewModel);
        }

        public IActionResult Demo()
        {
            return View();
        }



        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}