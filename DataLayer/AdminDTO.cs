using Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MySqlConnector;
using Dapper;
using Google.Protobuf.WellKnownTypes;

namespace DataLayer
{
    public class AdminDTO
    {

        public static async Task<Admin> Login(string Email, string password)
        {           
            string ConnectionString = @"server=localhost;user id=root;persistsecurityinfo=True;database=project;password=josh17rog";
            using (MySqlConnection connection = new MySqlConnection(ConnectionString))
            {

                string update = $"SELECT * FROM admin WHERE Password = '{password}' AND Email = '{Email}'";
                Admin ca = new Admin();
                try
                {
                    await connection.OpenAsync();

                    using (MySqlCommand cmd = new MySqlCommand(update, connection))
                    {
                        MySqlDataReader r = cmd.ExecuteReader();
                        
                        if (r.HasRows)
                        {
                            await r.ReadAsync();
                            int id = r.GetInt32(0);
                            ca = new Admin(id, r.GetString(1), r.GetString(7), r.GetString(4), new byte[0], r.GetString(6), r.GetString(2), new List<Project_Task>());

                        }
                        return ca;
                    }

                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.ToString());
                    return ca;
                }
            }
        }

        public static async Task<Admin> GetAdminById(int id)
        {
            string ConnectionString = @"server=localhost;user id=root;persistsecurityinfo=True;database=project;password=josh17rog";
            using (MySqlConnection connection = new MySqlConnection(ConnectionString))
            {

                string update = $"SELECT * FROM admin WHERE AdminID = '{id}'";
                Admin ca = new Admin();
                try
                {
                    await connection.OpenAsync();

                    using (MySqlCommand cmd = new MySqlCommand(update, connection))
                    {
                        MySqlDataReader r = cmd.ExecuteReader();

                        if (r.HasRows)
                        {
                            await r.ReadAsync();
                            ca = new Admin(id, r.GetString(1), r.GetString(7), r.GetString(4), await AdminDTO.GetProfilePhotoInByte(id), r.GetString(6), r.GetString(2), new List<Project_Task>());

                        }
                        return ca;
                    }

                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.ToString());
                    return ca;
                }
            }
        }

        public static async Task<List<Admin>> GetAllAdmins()
        {
            List<Admin> AdminList = new List<Admin>();

            // Use a using statement to ensure proper disposal of resources
            using (MySqlConnection c = new MySqlConnection())
            {
                try
                {
                    c.ConnectionString = @"server=localhost;user id=root;persistsecurityinfo=True;database=project;password=josh17rog";
                    await c.OpenAsync(); // Open the connection asynchronously

                    string query = @"SELECT * FROM admin";
                    using (MySqlCommand cmd = new MySqlCommand(query, c))
                    {
                        // Execute the query asynchronously
                        using (MySqlDataReader r = await cmd.ExecuteReaderAsync())
                        {
                            while (await r.ReadAsync())
                            {
                                int id = r.GetInt32(0); // Assuming UserID is at index 0 in the result set


                                // Fetch related data asynchronously using separate methods
                                Admin admin = new Admin(
                                    id,
                                    r.GetString(1),
                                    r.GetString(7),
                                    r.GetString(4),
                                    await GetProfilePhotoInByte(id),
                                    r.GetString(6),
                                    r.GetString(2),
                                    new List<Project_Task>()
                                );

                                AdminList.Add(admin);
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    // Handle exceptions appropriately (e.g., log, rethrow, etc.)
                    Console.WriteLine($"An error occurred: {ex.Message}");
                }
            }

            return AdminList;
        }

    
    public static async Task<byte[]> GetProfilePhotoInByte(int id)
        {
            string query = $"Select ProfilePhoto From admin Where AdminID ={id}";

            using (var connection = new MySqlConnection(@"server=localhost;user id=root;persistsecurityinfo=True;database=project;password=josh17rog"))
            {
                connection.Open();
                var result = connection.QueryFirstOrDefault<byte[]>(query);
                return result;
            }
        }
    }
}
