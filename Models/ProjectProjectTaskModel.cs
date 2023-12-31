namespace BCSH2_Sem_prace_Chyska.Models
{
    public class ProjectProjectTaskModel
    {
        public Project Project { get; set; }
        public List<ProjectTask> ProjectTasks { get; set; }
        public List<ProjectTaskStatus> Statuses { get; set; }
        public List<ProjectTaskPriority> Priorities { get; set; }
    }
}
