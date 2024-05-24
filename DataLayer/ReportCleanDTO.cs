using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Model;
using MySqlConnector;

namespace DataLayer
{
    public class ReportCleanDTO : BaseDTO
    {
        public async Task<List<ReportClean>> GetAllReportsForUser(int userID)
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
                                L.Add(new ReportClean(userID, trashcanID, weight, d, reportID));
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

        public override async Task<List<ReportClean>> SelectAll()
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

        public override async Task Insert(object o)
        {
            TrashCanDTO u = new TrashCanDTO();
            if (o is ReportClean)
            {
                ReportClean r = (ReportClean)o;
                await u.ReportClean(r.Userid, r.weight, r.TrashCanId);
            }
        }

        public override async Task Delete(int id)
        {
            try
            {
                using (MySqlConnection connection = new MySqlConnection(_connectionString))
                {
                    connection.Open();

                    string query = $"Delete from clean_report where ReportID ={id};";

                    using (MySqlCommand cmd = new MySqlCommand(query, connection))
                    {

                        await cmd.ExecuteNonQueryAsync();

                    }
                    connection.Close();

                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        public override async Task Update(object o)
        {
            if (o is ReportClean)
            {
                ReportClean r = (ReportClean)o;
                string ConnectionString = @"server=localhost;user id=root;persistsecurityinfo=True;database=project;password=josh17rog";
                using (MySqlConnection connection = new MySqlConnection(ConnectionString))
                {

                    string update = $"UPDATE clean_report SET  UserID = @userid, Weight = @Weight, Date = @date, TrashCanID = @trashcan WHERE ReportID ={r.ReportID}; ";

                    try
                    {
                        await connection.OpenAsync();

                        using (MySqlCommand UpdateCommand = new MySqlCommand(update, connection))
                        {
                            UpdateCommand.Parameters.AddWithValue("@userid", r.Userid);
                            UpdateCommand.Parameters.AddWithValue("@Weight", r.weight);
                            UpdateCommand.Parameters.AddWithValue("@date", r.date);
                            UpdateCommand.Parameters.AddWithValue("@trashcan", r.TrashCanId);

                            await UpdateCommand.ExecuteNonQueryAsync();
                        }

                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.ToString());
                    }
                }
            }

        }

        public override async Task<ReportClean> GetByPK(int id)
        {
            using (MySqlConnection c = new MySqlConnection(_connectionString))
            {
                await c.OpenAsync();

                string query = @"SELECT * FROM clean_report WHERE ReportID = @id";

                using (MySqlCommand cmd = new MySqlCommand(query, c))
                {
                    // Use parameters to prevent SQL injection
                    cmd.Parameters.AddWithValue("@id", id);

                    using (MySqlDataReader r = await cmd.ExecuteReaderAsync())
                    {
                        if (await r.ReadAsync())
                        {
                            return new ReportClean(r.GetInt32(1), r.GetInt32(4), r.GetInt32(2), r.GetDateTime(3), id);
                        }
                    }
                }
            }
            return null;
        }
    }
}
