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
        public int ID;
        public int Location;
        public int IsFull;
		public int Weight;
		public string Coordinates;

		public TrashCanDTO(int iD, int location, int isFull, int weight, string coordinates)
		{
			ID = iD;
			Location = location;
			IsFull = isFull;
			Weight = weight;
			Coordinates = coordinates;
		}

		public TrashCanDTO()
		{
			ID = -1;
			Location = -1;
			IsFull = -1;
			Weight = -1;
			Coordinates = "";
		}

        public static async Task ReportClean(int userid,int weight,int trashcanID)
        {
            try
            {
                DateTime now = DateTime.Now;
                string formattedDate = now.ToString("yyyy-MM-dd HH:mm:ss"); ;
                string q = $"Insert into clean_report (UserID,TrashCanID,Weight,Date) values ('{userid}','{trashcanID}','{weight}','{formattedDate}')";
                MySqlConnection c = new MySqlConnection();
                c.ConnectionString = @"server=localhost;user id=root;persistsecurityinfo=True;database=project;password=josh17rog";
                c.Open();
                MySqlCommand cmd = new MySqlCommand();
                cmd.Connection = c;
                cmd.CommandText = q;
                await cmd.ExecuteNonQueryAsync();
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }

        }

		private static string GetLatByPK(int id)
		{
            string connectionString = @"server=localhost;user id=root;persistsecurityinfo=True;database=project;password=josh17rog";
            int userId = id; // Replace with the UserID you want to use.

            string lat = "";
            string Lng = "";

            using (MySqlConnection connection = new MySqlConnection(connectionString))
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

        private static string GetLngByPK(int id)
        {
            string connectionString = @"server=localhost;user id=root;persistsecurityinfo=True;database=project;password=josh17rog";
            int userId = id; // Replace with the UserID you want to use.

            string Lng = "";

            using (MySqlConnection connection = new MySqlConnection(connectionString))
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

        public static Tuple<string,string> GetLatLngFromPK(int id)
        {
            string Lat = GetLatByPK(id);
            string lng = GetLngByPK(id);
            Tuple<string, string> t = new Tuple<string, string>(Lat, lng);
            return t;
        }

        public static int countRows()
        {
            string connectionString = @"server=localhost;user id=root;persistsecurityinfo=True;database=project;password=josh17rog";
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
        public static async Task<List<TrashCan>> GetAlltrashCanLocations()
        {
            int i = 1;
            string connectionString = @"server=localhost;user id=root;persistsecurityinfo=True;database=project;password=josh17rog";
            List<TrashCan> l = new List<TrashCan>(countRows());
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

        public static async Task InsertTrashCan(TrashCan trashCan)
        {
            string connectionString = @"server=localhost;user id=root;persistsecurityinfo=True;database=project;password=josh17rog"; 

            try
            {
                using (MySqlConnection connection = new MySqlConnection(connectionString))
                {
                    connection.Open();

                    string query = $"Insert into trashcan  (TrashCanWeight,Lat,Lng) VALUES ('{trashCan.weight.ToString()}','{trashCan.latitude}','{trashCan.longitude}');";

                    using (MySqlCommand cmd = new MySqlCommand(query, connection))
                    {

                        cmd.ExecuteNonQuery();

                    }
                    connection.Close();

                }
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            
        }

        public static async Task DeleteTrashCan(int id)
        {
            String connectionString = @"server=localhost;user id=root;persistsecurityinfo=True;database=project;password=josh17rog";

            try
            {
                using (MySqlConnection connection = new MySqlConnection(connectionString))
                {
                    connection.Open();

                    string query = $"Delete from trashcan where TrashCanID ={id};";

                    using (MySqlCommand cmd = new MySqlCommand(query, connection))
                    {

                        cmd.ExecuteNonQuery();

                    }
                    connection.Close();

                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        public static async Task UpdateTrashCan(TrashCan trashCan)
        {
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
        public async Task AddWeightAsync(int trashCanId, float newWeight)
        {
            using (var connection = CreateConnection())
            {
                await connection.OpenAsync();
                var currentWeight = await connection.QueryFirstOrDefaultAsync<float?>(
                    "SELECT TrashCanWeight FROM trashcan WHERE TrashCanId = @TrashCanId", new { TrashCanId = trashCanId });

                if (currentWeight.HasValue)
                {
                    var oldWeight = currentWeight.Value;
                    var weightDifference = newWeight;

                    using (var transaction = connection.BeginTransaction())
                    {
                        // Update the trash_can table
                        await connection.ExecuteAsync(
                            "UPDATE trashcan SET TrashCanWeight = @NewWeight WHERE TrashCanID = @TrashCanId",
                            new { TrashCanId = trashCanId, NewWeight = newWeight + oldWeight },
                            transaction: transaction);

                        // Insert into the weight_log table
                        await connection.ExecuteAsync(
                            @"INSERT INTO weight_log (TrashCanID, old_weight, new_weight, weight_difference,change_time)
                          VALUES (@TrashCanId, @OldWeight, @NewWeight, @WeightDifference,NOW())",
                            new { TrashCanId = trashCanId, OldWeight = oldWeight, NewWeight = newWeight + oldWeight, WeightDifference = newWeight },
                            transaction: transaction);

                        transaction.Commit();
                    }
                }
                else
                {
                    throw new Exception("Trash can not found");
                }
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
            using(var connection = CreateConnection())
            {
                await connection.OpenAsync();
                var currentWeight = await connection.QueryFirstOrDefaultAsync<float?>(
                    "SELECT TrashCanWeight FROM trashcan WHERE TrashCanId = @TrashCanId", new { TrashCanId = trashCanId });

                if (currentWeight.HasValue)
                {
                    var oldWeight = currentWeight.Value;
                    var weightDifference = weight-oldWeight;

                    using (var transaction = connection.BeginTransaction())
                    {
                        // Update the trash_can table
                        await connection.ExecuteAsync(
                            "UPDATE trashcan SET TrashCanWeight = @NewWeight WHERE TrashCanID = @TrashCanId",
                            new { TrashCanId = trashCanId, NewWeight = oldWeight- weight },
                            transaction: transaction);

                        // Insert into the weight_log table
                        await connection.ExecuteAsync(
                            @"INSERT INTO weight_log (TrashCanID, old_weight, new_weight, weight_difference,change_time)
                          VALUES (@TrashCanId, @OldWeight, @NewWeight, @WeightDifference,NOW())",
                            new { TrashCanId = trashCanId, OldWeight = oldWeight, NewWeight = -weight, WeightDifference = weightDifference },
                            transaction: transaction);

                        transaction.Commit();
                    }
                    return "Weight removal succefull";
                }
                else
                {
                    throw new Exception("Trash can not found");
                }
            }
        }

    }
}
