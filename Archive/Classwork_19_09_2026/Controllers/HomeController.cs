using Classwork_19_09_2026.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace Classwork_19_09_2026.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            var model = new PersonViewModel
            {
                Message = "Welcome to the Person View!",
            };
            return View(model);
        }

        [HttpPost]
        public IActionResult Index(PersonViewModel model)
        {
            if(!string.IsNullOrEmpty(model.Name) && model.BirthYear > 1920)
            {
                int currentYear = DateTime.Now.Year;
                int age = currentYear - model.BirthYear;
                model.AgeResult = $"Hello {model.Name}, you are {age} years old.";
            }
            else
            {
                model.AgeResult = "Please enter a valid name and birth year.";
                model.Message = "Error in input data";
            }
            return View(model);
        }

        public IActionResult Privacy()
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
