using Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MySqlConnector;

namespace DataLayer
{
    public class Project_TaskDTO
    {

        public static async Task<List<Project_Task>> GetAllCurrentTasks()
        {
            string ConnectionString = @"server=localhost;user id=root;persistsecurityinfo=True;database=project;password=josh17rog";
            List<Project_Task> projectTasks = new List<Project_Task>();
            try
            {
                using (MySqlConnection connection = new MySqlConnection(ConnectionString))
                {
                    string query = @"
                    SELECT
                     pt.idProject_Task AS TaskID,
                     pt.Name AS TaskName,
                         GROUP_CONCAT(DISTINCT a.AdminID) AS AdminIDs,
                        GROUP_CONCAT(DISTINCT a.Name) AS AdminNames,
                        pt.Completion,
                        pt.Budget
                FROM
                    admin_projects ap
                JOIN
                         project_task pt ON ap.Project_TaskID = pt.idProject_Task
                    JOIN
                    admin a ON ap.AdminID = a.AdminID
                    GROUP BY
                    pt.idProject_Task, pt.Name, pt.Completion, pt.Budget;";

                    MySqlCommand command = new MySqlCommand(query, connection);


                    connection.Open();

                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            Project_Task task = new Project_Task
                            {
                                ID = reader.GetInt32("TaskID"),
                                Name = reader.GetString("TaskName"),
                                Admins = new List<Admin>(),
                                Completion = reader.GetInt32("Completion"),
                                Budget = reader.GetInt32("Budget")
                            };

                            string[] adminIDs = reader.GetString("AdminIDs").Split(',');
                            foreach (string adminIdString in adminIDs)
                            {
                                int adminId = int.Parse(adminIdString);
                                Admin admin = await AdminDTO.GetAdminById(adminId);
                                if (admin != null)
                                {
                                    task.Admins.Add(admin);
                                }
                            }



                            projectTasks.Add(task);
                        }
                    }
                }

                

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);

            }
            return projectTasks;
        }

        public static async Task AddNewTask(Project_Task task)
        {
            string connectionString = @"server=localhost;user id=root;persistsecurityinfo=True;database=project;password=josh17rog";
            try
            {
                using (var connection = new MySqlConnection(connectionString))
                {
                    connection.Open();
                    string insertProjectTaskQuery = "INSERT INTO Project_Task (Name, Completion, Budget) VALUES ( @Name, @Completion, @Budget)";
                    using (var cmd = new MySqlCommand(insertProjectTaskQuery, connection))
                    {
                        cmd.Parameters.AddWithValue("@Name", task.Name);
                        cmd.Parameters.AddWithValue("@Completion", task.Completion);
                        cmd.Parameters.AddWithValue("@Budget", task.Budget);
                        cmd.ExecuteNonQuery();
                    }
                    string getLastInsertIdQuery = "SELECT LAST_INSERT_ID()";
                    MySqlCommand getLastInsertIdCommand = new MySqlCommand(getLastInsertIdQuery, connection);
                    int taskid = (Int32)(getLastInsertIdCommand.ExecuteScalar());

                    foreach (Admin adminId in task.Admins)
                    {
                        string insertAdminProjectQuery = "INSERT INTO Admin_Projects (AdminID, Project_TaskID) VALUES (@AdminID, @Project_TaskID)";
                        using (var cmd = new MySqlCommand(insertAdminProjectQuery, connection))
                        {
                            cmd.Parameters.AddWithValue("@AdminID", adminId.Id);
                            cmd.Parameters.AddWithValue("@Project_TaskID", task.ID);
                            cmd.ExecuteNonQuery();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

        }



    }
}
