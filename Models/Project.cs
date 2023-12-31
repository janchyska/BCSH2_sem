using BCSH2_Sem_prace_Chyska.Controllers;
using System.Data.SqlClient;

namespace BCSH2_Sem_prace_Chyska.Models
{
    public class Project
    {
        public int Id { get; set; } = 0;
        public string Name { get; set; }
        public string? Description { get; set; }
        public List<ProjectTask>? Tasks { get; set; }
        public string Owner { get; set; }

        public Project() {
            Tasks = new List<ProjectTask>();
            Id++;
        }

        public Project(int id, string projectName, string? projectDescription) {
            Id = id;
            Name = projectName; 
            Description = projectDescription;
            Tasks = new List<ProjectTask>();
            Owner = string.Empty;
        }     
        
        public Project(string name, string? description) {
            Id = Id++;
            Name = name; 
            Description = description;
            Tasks = new List<ProjectTask>();
            Owner = string.Empty;
        }
    }
}
