using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime;
using System.Text;
using System.Threading.Tasks;
using Dapper;
using Google.Protobuf.WellKnownTypes;
using Model;
using MySqlConnector;

namespace DataLayer
{
    public class TrashCanDTO:BaseDTO
    {


		public TrashCanDTO()
		{
	
		}

        public async Task ReportClean(int userid,int weight,int trashcanID)
        {
            const string query = "INSERT INTO clean_report (UserID, TrashCanID, Weight, Date) VALUES (@UserId, @TrashCanId, @Weight, @Date)";

            try
            {
                using (var connection = new MySqlConnection(_connectionString))
                {
                    await connection.OpenAsync();

                    using (var command = new MySqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@UserId", userid);
                        command.Parameters.AddWithValue("@TrashCanId", trashcanID);
                        command.Parameters.AddWithValue("@Weight", weight);
                        command.Parameters.AddWithValue("@Date", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));

                        await command.ExecuteNonQueryAsync();
                    }
                }
            }
            catch (Exception ex)
            {
                // Consider more sophisticated logging in a real-world application
                Console.WriteLine(ex.ToString());
            }

        }

		private  string GetLatByPK(int id)
		{
            try
            {


                string lat = "";
                string Lng = "";

                using (MySqlConnection connection = new MySqlConnection(_connectionString))
                {
                    connection.Open();

                    string query = $"SELECT Lat from trashcan where TrashCanID ={id};";

                    using (MySqlCommand cmd = new MySqlCommand(query, connection))
                    {


                        using (MySqlDataReader reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                lat = reader.GetString(0);
                            }
                        }
                    }
                    connection.Close();
                    return lat;
                }
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.ToString());
                return null;
            }




        }

        private  string GetLngByPK(int id)
        {


            string Lng = "";

            using (MySqlConnection connection = new MySqlConnection(_connectionString))
            {
                connection.Open();

                string query = $"SELECT Lng from trashcan where TrashCanID ={id};";

                using (MySqlCommand cmd = new MySqlCommand(query, connection))
                {


                    using (MySqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            Lng = reader.GetString(0);
                        }
                    }
                }
                connection.Close();
                return Lng;
            }
        }
        public override async Task<TrashCan> GetByPK(int id)
        {
            using (MySqlConnection c = new MySqlConnection(_connectionString))
            {
                await c.OpenAsync();

                string query = @"SELECT * FROM trashcan WHERE TrashCanID = @id";

                using (MySqlCommand cmd = new MySqlCommand(query, c))
                {
                    // Use parameters to prevent SQL injection
                    cmd.Parameters.AddWithValue("@id", id);

                    using (MySqlDataReader r = await cmd.ExecuteReaderAsync())
                    {
                        if (await r.ReadAsync())
                        {
                            return new TrashCan(id, r.GetInt32(1), r.GetString(2), r.GetString(3));
                        }
                    }
                }
            }
            return null;
        }

        public  Tuple<string,string> GetLatLngFromPK(int id)
        {
            string Lat = GetLatByPK(id);
            string lng = GetLngByPK(id);
            Tuple<string, string> t = new Tuple<string, string>(Lat, lng);
            return t;
        }

        public int countRows()
        {
            string connectionString = _connectionString;
            int num = 0;

            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                connection.Open();

                string query = $"SELECT * from trashcan;";

                using (MySqlCommand cmd = new MySqlCommand(query, connection))
                {


                    using (MySqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            num = num + 1;
                           
                        }
                    }
                }
               
                return num;
            }
        }
        public override  async Task<List<TrashCan>> SelectAll()
        {
            List<TrashCan> l = new List<TrashCan>(countRows());
            using (MySqlConnection connection = new MySqlConnection(_connectionString))
            {
                connection.Open();

                string query = $"SELECT * from trashcan;";

                using (MySqlCommand cmd = new MySqlCommand(query, connection))
                {


                    using (MySqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            int id = reader.GetInt32(0);
                            int weight = reader.GetInt32(1);
                            string lng = reader.GetString(3);
                            string lat = reader.GetString(2);
                            l.Add(new TrashCan(id,weight,lng,lat));

                        }
                    }
                }

               
            }

            return l;

        }

        public override  async Task Insert(object o)
        {
            TrashCan trashCan =(TrashCan)o;
            string connectionString = @"server=localhost;user id=root;persistsecurityinfo=True;database=project;password=josh17rog"; 

            try
            {
                using (MySqlConnection connection = new MySqlConnection(connectionString))
                {
                    connection.Open();

                    string query = $"Insert into trashcan  (TrashCanWeight,Lat,Lng) VALUES ('{trashCan.weight.ToString()}','{trashCan.latitude}','{trashCan.longitude}');";

                    using (MySqlCommand cmd = new MySqlCommand(query, connection))
                    {

                        await cmd.ExecuteNonQueryAsync();

                    }
                    connection.Close();

                }
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            
        }

        public  override async Task Delete(int id)
        {
            

            try
            {
                using (MySqlConnection connection = new MySqlConnection(_connectionString))
                {
                    connection.Open();

                    string query = $"Delete from trashcan where TrashCanID ={id};";

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
            TrashCan trashCan = (TrashCan)o;
            string ConnectionString = @"server=localhost;user id=root;persistsecurityinfo=True;database=project;password=josh17rog";
            using (MySqlConnection connection = new MySqlConnection(ConnectionString))
            {

                string update = $"UPDATE trashcan SET  lat = @lat, TrashCanWeight = @Weight, lng = @lng WHERE TrashCanID = {trashCan.id}; ";

                try
                {
                    await connection.OpenAsync();

                    using (MySqlCommand UpdateUserCommand = new MySqlCommand(update, connection))
                    {
                        UpdateUserCommand.Parameters.AddWithValue("@lat", trashCan.latitude);
                        UpdateUserCommand.Parameters.AddWithValue("@lng", trashCan.longitude);
                        UpdateUserCommand.Parameters.AddWithValue("@Weight",trashCan.weight);

                        await UpdateUserCommand.ExecuteNonQueryAsync();
                    }

                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.ToString());
                }
            }

        }
        public async Task<string> AddWeightAsync(int trashCanId, float newWeight)
        {
            try
            {
                using (var connection = new MySqlConnection(_connectionString))
                {
                    await connection.OpenAsync();

                    // Fetch the current weight
                    string selectQuery = "SELECT TrashCanWeight FROM trashcan WHERE TrashCanId = @TrashCanId";
                    using (var selectCommand = new MySqlCommand(selectQuery, connection))
                    {
                        selectCommand.Parameters.AddWithValue("@TrashCanId", trashCanId);

                        var result = await selectCommand.ExecuteScalarAsync();
                        if (result == null)
                        {
                            throw new Exception("Trash can not found");
                        }

                        float oldWeight = Convert.ToSingle(result);
                        float updatedWeight = newWeight + oldWeight;
                        float weightDifference = newWeight;

                        // Update the trashcan table
                        string updateQuery = "UPDATE trashcan SET TrashCanWeight = @NewWeight WHERE TrashCanID = @TrashCanId";
                        using (var updateCommand = new MySqlCommand(updateQuery, connection))
                        {
                            updateCommand.Parameters.AddWithValue("@TrashCanId", trashCanId);
                            updateCommand.Parameters.AddWithValue("@NewWeight", updatedWeight);

                            await updateCommand.ExecuteNonQueryAsync();
                        }

                        // Insert into the weight_log table
                        string insertQuery = @"INSERT INTO weight_log (TrashCanID, old_weight, new_weight, weight_difference, change_time)
                                       VALUES (@TrashCanId, @OldWeight, @NewWeight, @WeightDifference, NOW())";
                        using (var insertCommand = new MySqlCommand(insertQuery, connection))
                        {
                            insertCommand.Parameters.AddWithValue("@TrashCanId", trashCanId);
                            insertCommand.Parameters.AddWithValue("@OldWeight", oldWeight);
                            insertCommand.Parameters.AddWithValue("@NewWeight", updatedWeight);
                            insertCommand.Parameters.AddWithValue("@WeightDifference", weightDifference);

                            await insertCommand.ExecuteNonQueryAsync();
                        }

                        return "Weight update successful";
                    }
                }
            }
            catch (Exception ex)
            {
                // Log the exception (consider using a logging framework)
                Console.WriteLine(ex.Message);
                // Optionally, handle the exception appropriately (e.g., rethrow, return an error message, etc.)
                throw;
            }
        }

        public async Task<bool> HasWeightChangedAsync(int trashCanId, DateTime lastCheckTime)
        {
            string connectionString = @"server=localhost;user id=root;persistsecurityinfo=True;database=project;password=josh17rog";
            try
            {
                using (MySqlConnection connection = new MySqlConnection(connectionString))
                {
                    await connection.OpenAsync();

                    string query = @"
                SELECT COUNT(*)
                FROM weight_log
                WHERE TrashCanID = @TrashCanId
                AND change_time > @LastCheckTime";

                    int count = await connection.ExecuteScalarAsync<int>(query, new { TrashCanId = trashCanId, LastCheckTime = lastCheckTime });
                    return count > 0;
                }
            }
            catch (MySqlException ex)
            {
                Console.WriteLine("MySQL Error: " + ex.Message);
                // Log the error or handle it appropriately
                return false;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error: " + ex.Message);
                // Log the error or handle it appropriately
                return false;
            }
        }

        public async Task<int> CheckForNewEntryAsync(int trashCanId)
        {
            try
            {
                string _connectionString = @"server=localhost;user id=root;persistsecurityinfo=True;database=project;password=josh17rog";
                using (var connection = new MySqlConnection(_connectionString))
                {
                    await connection.OpenAsync();

                    // Query to check if there is a new entry in the weight log within the last 30 minutes
                    string query = @"
                SELECT weight_difference
                FROM weight_log
                WHERE TrashCanID = @TrashCanID
                AND change_time > DATE_SUB(NOW(), INTERVAL 30 SECOND) AND weight_difference>0
                ORDER BY change_time DESC
                LIMIT 1";

                    using (var command = new MySqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@TrashCanID", trashCanId);
                        var result = await command.ExecuteScalarAsync();

                        if (result != null)
                        {
                            return Convert.ToInt32(result);
                        }
                        else
                        {
                            return -1; // No new entry found
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                // Handle exceptions here, such as logging or returning false
                Console.WriteLine($"Error checking for new entry: {ex.Message}");
                return -1;
            }
        }
        public   async Task<string> RemoveWeightAsync(int trashCanId, int weight)
        {
            try
            {
                using (var connection = new MySqlConnection(_connectionString))
                {
                    await connection.OpenAsync();

                    string selectQuery = "SELECT TrashCanWeight FROM trashcan WHERE TrashCanId = @TrashCanId";
                    using (var selectCommand = new MySqlCommand(selectQuery, connection))
                    {
                        selectCommand.Parameters.AddWithValue("@TrashCanId", trashCanId);

                        var result = await selectCommand.ExecuteScalarAsync();
                        if (result == null)
                        {
                            throw new Exception("Trash can not found");
                        }

                        float oldWeight = Convert.ToSingle(result);
                        float newWeight = oldWeight - weight;
                        float weightDifference = weight;

                        string updateQuery = "UPDATE trashcan SET TrashCanWeight = @NewWeight WHERE TrashCanID = @TrashCanId";
                        using (var updateCommand = new MySqlCommand(updateQuery, connection))
                        {
                            updateCommand.Parameters.AddWithValue("@TrashCanId", trashCanId);
                            updateCommand.Parameters.AddWithValue("@NewWeight", newWeight);

                            await updateCommand.ExecuteNonQueryAsync();
                        }

                        string insertQuery = @"INSERT INTO weight_log (TrashCanID, old_weight, new_weight, weight_difference, change_time)
                                       VALUES (@TrashCanId, @OldWeight, @NewWeight, @WeightDifference, NOW())";
                        using (var insertCommand = new MySqlCommand(insertQuery, connection))
                        {
                            insertCommand.Parameters.AddWithValue("@TrashCanId", trashCanId);
                            insertCommand.Parameters.AddWithValue("@OldWeight", oldWeight);
                            insertCommand.Parameters.AddWithValue("@NewWeight", newWeight);
                            insertCommand.Parameters.AddWithValue("@WeightDifference", weightDifference);

                            await insertCommand.ExecuteNonQueryAsync();
                        }

                        return "Weight removal successful";
                    }
                }
            }
            catch (Exception ex)
            {
                // Log the exception (consider using a logging framework)
                Console.WriteLine(ex.Message);
                // Optionally, handle the exception appropriately (e.g., rethrow, return an error message, etc.)
                throw;
            }
        }

    }
}
