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
                     pt.ID AS TaskID,
                     pt.Name AS TaskName,
                         GROUP_CONCAT(DISTINCT a.ID) AS AdminIDs,
                        GROUP_CONCAT(DISTINCT a.NAME) AS AdminNames,
                        pt.Completion,
                        pt.Budget
                FROM
                    admin_project ap
                JOIN
                         project_task pt ON ap.Project_TaskID = pt.ID
                    JOIN
                    admin a ON ap.AdminID = a.ID
                    GROUP BY
                    pt.ID, pt.Name, pt.Completion, pt.Budget;";

                    MySqlCommand command = new MySqlCommand(query, connection);
                    command.CommandType = System.Data.CommandType.StoredProcedure;

                    connection.Open();

                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            Project_Task task = new Project_Task
                            {
                                ID = reader.GetInt32("ID"),
                                Name = reader.GetString("Name"),
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
            
    }
}
