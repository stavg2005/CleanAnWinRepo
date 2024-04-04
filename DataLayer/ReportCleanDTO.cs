using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Model;
using MySqlConnector;

namespace DataLayer
{
    public class ReportCleanDTO
    {
        public static async Task<List<ReportClean>> GellAllReports(int userID)
        {
            string connectionString = @"server=localhost;user id=root;persistsecurityinfo=True;database=project;password=josh17rog";

            List<ReportClean> L = new List<ReportClean>();
            try
            {
                using (MySqlConnection connection = new MySqlConnection(connectionString))
                {
                    connection.Open();

                    string query = $"SELECT ReportID, Weight, Date, TrashCanID FROM clean_report WHERE UserID={userID};";

                    using (MySqlCommand cmd = new MySqlCommand(query, connection))
                    {


                        using (MySqlDataReader reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                int reportID = reader.GetInt32(0);
                                int trashcanID = reader.GetInt32(3);
                                int weight = reader.GetInt32(1);
                                DateTime d = reader.GetDateTime(2);
                                L.Add(new ReportClean(userID, trashcanID, weight,d,reportID));
                            }
                        }
                    }
                    connection.Close();
                    
                }
                return L;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return new List<ReportClean>();
            }
           
        }

        public static async Task<List<ReportClean>> GellAllReports()
        {
            string connectionString = @"server=localhost;user id=root;persistsecurityinfo=True;database=project;password=josh17rog";

            List<ReportClean> L = new List<ReportClean>();
            try
            {
                using (MySqlConnection connection = new MySqlConnection(connectionString))
                {
                    connection.Open();

                    string query = $"SELECT * FROM clean_report";

                    using (MySqlCommand cmd = new MySqlCommand(query, connection))
                    {


                        using (MySqlDataReader reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                int reportID = reader.GetInt32(0);
                                int UserID = reader.GetInt32(1);
                                int trashcanID = reader.GetInt32(4);
                                int weight = reader.GetInt32(2);
                                DateTime d = reader.GetDateTime(3);
                                L.Add(new ReportClean(UserID, trashcanID, weight, d, reportID));
                            }
                        }
                    }
                    connection.Close();

                }
                return L;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return new List<ReportClean>();
            }

        }

    }
}
