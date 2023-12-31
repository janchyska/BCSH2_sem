using BCSH2_Sem_prace_Chyska.Models;
using Microsoft.AspNetCore.Mvc;
using System.Data.SqlClient;
using System.Threading.Tasks;

namespace BCSH2_Sem_prace_Chyska.Controllers
{
    public class ProjectController : Controller
    {

        public static List<ProjectTask> ProjectTasks = new List<ProjectTask>();
        DatabaseHelpers db = new DatabaseHelpers();

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Details(int id)
        {

            Project project = HomeController.Projects.Find(p => p.Id == id);
            List<ProjectTask> projectTasks = db.GetAllProjectTasks(id);
            List<ProjectTaskStatus> statuses = db.GetStatuses();
            List<ProjectTaskPriority> priorities = db.GetPriorities(); 

            var viewModel = new ProjectProjectTaskModel
            {
                Project = project,
                ProjectTasks = projectTasks,
                Statuses = statuses,
                Priorities = priorities
            };
            if (project == null)
            {
                return NotFound();
            }
            return View(viewModel);
        }


        [HttpPost]
        public IActionResult AddTask(int projectID, ProjectTask model)
        {
            Project project = HomeController.Projects.Find(p => p.Id == projectID);
            List<ProjectTaskStatus> statuses = db.GetStatuses();
            List<ProjectTaskPriority> priorities = db.GetPriorities();
            ProjectTaskStatus status = statuses.Find(p =>p.Id == model.SelectedStatusId);
            ProjectTaskPriority priority = priorities.Find(p =>p.Id == model.SelectedPriorityId);
            ProjectTask projectTask = new ProjectTask
            {
                Name = model.Name,
                Description = model.Description,
                Priority = priority,
                Status = status,
                Deadline = model.Deadline,
                Project = project
            };
            ProjectTasks.Add(projectTask);
            db.InsertTask(projectTask);
            string returnUrl = Url.Action("Details", "Project", new { id = projectID });

            return Redirect(returnUrl);
        }

        public IActionResult DeleteTask(ProjectTask task, int projectID)
        {
            if (task == null)
            {
                return NotFound();
            }
            int id = task.Project.Id;
            db.DeleteTask(task.Id);
            ProjectTasks.Remove(task);
            string returnUrl = Url.Action("Details", "Project", new { id = projectID });
            return Redirect(returnUrl);
        }
    }
}
