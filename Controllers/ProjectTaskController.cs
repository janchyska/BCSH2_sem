using BCSH2_Sem_prace_Chyska.Models;
using Microsoft.AspNetCore.Mvc;

namespace BCSH2_Sem_prace_Chyska.Controllers
{
    public class ProjectTaskController : Controller
    {
        DatabaseHelpers db = new DatabaseHelpers();

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Detail(int id, int projectID)
        {
            List<ProjectTask> list = db.GetAllProjectTasks(projectID);
            ProjectTask task = list.Find(p => p.Id == id);
            return View(task);
        }

        [HttpGet]
        public IActionResult EditTask(int id, int projectID)
        {
            List<ProjectTask> tasks = db.GetAllProjectTasks(projectID);
            ProjectTask task = tasks.FirstOrDefault(t => t.Id == id);
            if (task == null)
            {
                return NotFound();
            }
            List<ProjectTaskStatus> statuses = db.GetStatuses();
            List<ProjectTaskPriority> priorities = db.GetPriorities();

            var viewModel = new EditTaskViewModel
            {
                Task = task,
                Statuses = statuses,
                Priorities = priorities
            };

            return View(viewModel);
        }

        [HttpPost]
        public IActionResult EditTask(int id, int projectID, ProjectTask model)
        {
            db.UpdateTask(model);
            string returnUrl = Url.Action("Details", "Project", new { id = projectID });
            return Redirect(returnUrl);
        }

    }
}
