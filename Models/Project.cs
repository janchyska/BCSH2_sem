using BCSH2_Sem_prace_Chyska.Controllers;
using System.Data.SqlClient;

namespace BCSH2_Sem_prace_Chyska.Models
{
    public class Project
    {
        public int Id { get; set; } = 0;
        public string Name { get; set; }
        public string? Description { get; set; }
        public List<ProjectTask> Tasks { get; set; }
        public string Owner { get; set; }

        public Project() { }

        public Project(int id, string name, string? description) {
            Id = id;
            Name = name; 
            Description = description;
            Tasks = new List<ProjectTask>();
            Owner ="test";
        }     
               

        public Project NewProject(string name, string? description)
        {
            Project project = null;

                DatabaseHelpers.ExecuteNonQuery("INSERT INTO PROJECT (Name, Description, Owner) VALUES (@Name, @Description, @Owner)", new System.Data.SqlClient.SqlParameter[]
                {
                new SqlParameter("@Name", name),
                new SqlParameter("@Description", description),
                new SqlParameter("@Owner", "testuser")
                });
            int id = Convert.ToInt16(DatabaseHelpers.ExecuteNonQuery("SELECT SCOPE_IDENTITY()"));
            project = new Project(id, name, description);
            return project;
        }    
        
        public Project DeleteProject()
        {

            return null;
        }
    }
}
