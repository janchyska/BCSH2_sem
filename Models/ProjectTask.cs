using BCSH2_Sem_prace_Chyska.Controllers;
using System.ComponentModel.DataAnnotations;

namespace BCSH2_Sem_prace_Chyska.Models
{
    public class ProjectTask
    {
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        public string? Description { get; set; }
        public ProjectTaskPriority? Priority { get; set; }
        public ProjectTaskStatus? Status { get; set; }
        public DateTime? Deadline { get; set; }
        public Project Project { get; set; }
        public int? SelectedStatusId { get; set; }
        public int? SelectedPriorityId { get; set; }

        public ProjectTask() { 
            Project = new Project();
        }

        public ProjectTask (Project project, string name, string description, ProjectTaskPriority priority, ProjectTaskStatus status, DateTime deadline)
        {
            Id++;
            Project = project;
            Name = name;
            Description = description;
            Priority = priority;
            Status = status;
            Deadline = deadline;
        }   
        
        public ProjectTask (int id, Project project, string name, string description, ProjectTaskPriority priority, ProjectTaskStatus status, DateTime deadline)
        {
            Id = id;
            Project = project;
            Name = name;
            Description = description;
            Priority = priority;
            Status = status;
            Deadline = deadline;
        }
    }



}
