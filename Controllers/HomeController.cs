using BCSH2_Sem_prace_Chyska.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace BCSH2_Sem_prace_Chyska.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;

        }

        public IActionResult Index()
        {
            Project project = new Project();
            project.NewProject("test", "popis");
            Console.WriteLine("yes");
            return View();
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