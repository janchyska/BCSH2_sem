using BCSH2_Sem_prace_Chyska.Models;
using Microsoft.Extensions.Configuration;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;

namespace BCSH2_Sem_prace_Chyska.Controllers
{
    public class DatabaseHelpers
    {

        private readonly string _connectionString;
        public DatabaseHelpers()
        {
            var configuration = new ConfigurationBuilder()
                       .SetBasePath(Directory.GetCurrentDirectory())
                       .AddJsonFile("appsettings.json")
                       .Build();

            _connectionString = configuration.GetConnectionString("ProjectDatabase");
        }

        public  List<Project> GetAllProjects()
        {
            List<Project> projects = new List<Project>();

            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                connection.Open();

                string sqlQuery = "SELECT * FROM PROJECT";

                using (SqlCommand command = new SqlCommand(sqlQuery, connection))
                {
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            Project project = new Project
                            {
                                Id = Convert.ToInt32(reader["Id"]),
                                Name = reader["Name"].ToString(),
                                Description = reader["Description"] is DBNull ? null : reader["Description"].ToString(),
                                Owner = reader["Owner"].ToString(),
                                Tasks = new List<ProjectTask>()
                            };
                            projects.Add(project);
                        }
                    }
                }
            }
            return projects;
        }

        public void InsertProject(Project project)
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                connection.Open();

                string sqlQuery = "INSERT INTO PROJECT (Name, Description, Owner) VALUES (@Name, @Description, @Owner)";

                using (SqlCommand command = new SqlCommand(sqlQuery, connection))
                {
                    command.Parameters.AddWithValue("@Name", project.Name);
                    command.Parameters.AddWithValue("@Description", project.Description ?? (object)DBNull.Value);
                    command.Parameters.AddWithValue("@Owner", project.Owner);

                    command.ExecuteNonQuery();
                }
            }
        }

        public void DeleteProject(int projectId)
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                connection.Open();

                string sqlQuery = "DELETE FROM PROJECT WHERE Id = @ProjectId";

                using (SqlCommand command = new SqlCommand(sqlQuery, connection))
                {
                    command.Parameters.AddWithValue("@ProjectId", projectId);

                    command.ExecuteNonQuery();
                }
            }
        }


        public List<ProjectTask> GetAllProjectTasks(int projectID)
        {
            List<ProjectTask> tasks = new List<ProjectTask>();

            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                connection.Open();

                string sqlQuery = @"
            SELECT T.*, TS.Color AS TaskStatusColor, TS.Name AS TaskStatusName, TP.Name AS TaskPriorityName
            FROM TASK T
            LEFT JOIN TASKSTATUS TS ON T.TASKSTATUS_ID = TS.Id
            LEFT JOIN TASK_PRIORITY TP ON T.TASK_PRIORITY_ID = TP.Id
            WHERE T.PROJECT_ID = @ProjectID";

                using (SqlCommand command = new SqlCommand(sqlQuery, connection))
                {
                    command.Parameters.AddWithValue("@ProjectID", projectID);
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            string deadlineString = reader["Deadline"] is DBNull ? null : reader["Deadline"].ToString();
                            DateTime? deadline = string.IsNullOrEmpty(deadlineString) ? (DateTime?)null : DateTime.Parse(deadlineString);

                            int? taskStatusID = reader["TASKSTATUS_ID"] as int?;
                            int? taskPriorityID = reader["TASK_PRIORITY_ID"] as int?;

                            ProjectTaskStatus taskStatus = null;
                            if (taskStatusID.HasValue)
                            {
                                taskStatus = new ProjectTaskStatus
                                {
                                    Id = taskStatusID.Value,
                                    Name = reader["TaskStatusName"].ToString(),
                                    Color = reader["TaskStatusColor"] is DBNull ? null : reader["TaskStatusColor"].ToString()
                                };
                            }

                            ProjectTaskPriority taskPriority = null;
                            if (taskPriorityID.HasValue)
                            {
                                taskPriority = new ProjectTaskPriority
                                {
                                    Id = taskPriorityID.Value,
                                    Name = reader["TaskPriorityName"].ToString()
                                };
                            }

                            ProjectTask task = new ProjectTask
                            {
                                Id = Convert.ToInt32(reader["Id"]),
                                Name = reader["Name"].ToString(),
                                Description = reader["Description"] is DBNull ? null : reader["Description"].ToString(),
                                Deadline = deadline,
                                Status = taskStatus,
                                Priority = taskPriority,
                                Project = HomeController.Projects.Find(p => p.Id == projectID)
                            };
                            tasks.Add(task);
                        }
                    }
                }
            }
            return tasks;
        }

        public void InsertTask(ProjectTask task)
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                connection.Open();

                string sqlQuery = "INSERT INTO TASK (Name, Description, Deadline, PROJECT_ID, TASKSTATUS_ID, TASK_PRIORITY_ID) " +
                                  "VALUES (@Name, @Description, @Deadline, @ProjectId, @TaskStatusId, @TaskPriorityId);";

                using (SqlCommand command = new SqlCommand(sqlQuery, connection))
                {
                    command.Parameters.AddWithValue("@Name", task.Name);
                    command.Parameters.AddWithValue("@Description", task.Description ?? (object)DBNull.Value);
                    command.Parameters.AddWithValue("@Deadline", task.Deadline ?? (object)DBNull.Value);
                    command.Parameters.AddWithValue("@ProjectId", task.Project.Id);
                    command.Parameters.AddWithValue("@TaskStatusId", task.Status?.Id ?? (object)DBNull.Value);
                    command.Parameters.AddWithValue("@TaskPriorityId", task.Priority?.Id ?? (object)DBNull.Value);

                    command.ExecuteNonQuery();
                }
            }
        }


        public List<ProjectTaskPriority> GetPriorities()
        {
            List<ProjectTaskPriority> priorities = new List<ProjectTaskPriority>();

            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                connection.Open();

                string sqlQuery = "SELECT Id, Name FROM TASK_PRIORITY";

                using (SqlCommand command = new SqlCommand(sqlQuery, connection))
                {
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            ProjectTaskPriority priority = new ProjectTaskPriority
                            {
                                Id = reader.GetInt32(0),
                                Name = reader.GetString(1)
                            };
                            priorities.Add(priority);
                        }
                    }
                }
            }

            return priorities;
        }
        public List<ProjectTaskStatus> GetStatuses()
        {
            List<ProjectTaskStatus> statuses = new List<ProjectTaskStatus>();

            // Replace this with your actual database query to fetch statuses
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                string sqlQuery = "SELECT Id, Name, Color FROM TaskStatus";

                using (SqlCommand command = new SqlCommand(sqlQuery, connection))
                {
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            ProjectTaskStatus status = new ProjectTaskStatus
                            {
                                Id = reader.GetInt32(0),
                                Name = reader.GetString(1),
                                Color = reader.GetString(2)
                            };
                            statuses.Add(status);
                        }
                    }
                }
            }

            return statuses;
        }

        public void DeleteTask(int taskID)
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                connection.Open();

                string sqlQuery = "DELETE FROM TASK WHERE Id = @TaskID";

                using (SqlCommand command = new SqlCommand(sqlQuery, connection))
                {
                    command.Parameters.AddWithValue("@TaskID", taskID);

                    command.ExecuteNonQuery();
                }
            }
        }

        public void UpdateTask(ProjectTask task)
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                connection.Open();

                string sqlQuery = "UPDATE TASK " +
                                  "SET Name = @Name, Description = @Description, " +
                                  "Deadline = @Deadline, TASKSTATUS_ID = @TaskStatusId, TASK_PRIORITY_ID = @TaskPriorityId " +
                                  "WHERE Id = @Id";

                using (SqlCommand command = new SqlCommand(sqlQuery, connection))
                {
                    command.Parameters.AddWithValue("@Id", task.Id);
                    command.Parameters.AddWithValue("@Name", task.Name);
                    command.Parameters.AddWithValue("@Description", task.Description ?? (object)DBNull.Value);
                    command.Parameters.AddWithValue("@Deadline", task.Deadline ?? (object)DBNull.Value);
                    command.Parameters.AddWithValue("@TaskStatusId", task.SelectedStatusId ?? (object)DBNull.Value);
                    command.Parameters.AddWithValue("@TaskPriorityId", task.SelectedPriorityId ?? (object)DBNull.Value);

                    command.ExecuteNonQuery();
                }
            }
        }



    }

}
