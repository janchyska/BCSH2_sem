using BCSH2_Sem_prace_Chyska.Models;
using Microsoft.AspNetCore.Mvc;
using System.Data.SqlClient;
using System.Diagnostics;

namespace BCSH2_Sem_prace_Chyska.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        public static List<Project> Projects;
        DatabaseHelpers db;

        public HomeController(ILogger<HomeController> logger)
        {
            db = new DatabaseHelpers();
            Projects = new List<Project>();
            db.GetAllProjects().ForEach(project => { Projects.Add(project); });
        }

        public IActionResult Index()
        {
            if (TempData.ContainsKey("ErrorMessage"))
            {
                ViewBag.ErrorMessage = TempData["ErrorMessage"];
            }
            return View(Projects);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [HttpPost]
        public IActionResult AddProject(Project project)
        {
            if (ModelState.IsValid)
            {
                db.InsertProject(project);
                Projects.Add(project);
                return RedirectToAction("Index");
            }
            return View("Index", HomeController.Projects);
        }

        public IActionResult DeleteProject(Project project)
        {
            List<ProjectTask> tasks = db.GetAllProjectTasks(project.Id);
            if (tasks.Count > 0)
            {
                TempData["ErrorMessage"] = "Nemůžete smazat projekt, protože obsahuje úkoly.";
            }
            else
            {
                db.DeleteProject(project.Id);
                Projects.Remove(project);
            }
                return RedirectToAction("Index");
        }


        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }


    }
}