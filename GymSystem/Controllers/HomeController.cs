using System.Diagnostics;
using GymSystem.Models;
using Microsoft.AspNetCore.Mvc;

namespace GymSystem.Controllers
{
    public class HomeController : Controller
    {   
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

    }
}
