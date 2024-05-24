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
    public class AdminDTO:UsersDTO
    {

        

        public  async Task<Users> LoginAdmin(string email ,string password)
        {
            
            
            Users admin =await Login(email, password);
            if(admin != null && admin.IsAdmin)
            {
                return admin;
            }
            return null;

            
        }

        public  async Task<List<Users>> GetAllAdmins()
        {
            List<Users> usersList = new List<Users>();

            // Use a using statement to ensure proper disposal of resources
            using (MySqlConnection c = new MySqlConnection())
            {
                try
                {
                    c.ConnectionString = @"server=localhost;user id=root;persistsecurityinfo=True;database=project;password=josh17rog";
                    await c.OpenAsync(); // Open the connection asynchronously

                    string query = @"SELECT * FROM users Where IsAdmin =1";
                    using (MySqlCommand cmd = new MySqlCommand(query, c))
                    {
                        // Execute the query asynchronously
                        using (MySqlDataReader r = await cmd.ExecuteReaderAsync())
                        {
                            while (await r.ReadAsync())
                            {
                                int id = r.GetInt32(0); // Assuming UserID is at index 0 in the result set


                                // Fetch related data asynchronously using separate methods
                                Users user = new Users(
                                    id,
                                    r.GetString(1),
                                    r.GetInt32(3),
                                    r.GetString(4),
                                    r.GetInt32(5),
                                    await l.GetByPK(r.GetInt32(6)),
                                    await GetUserCart(id),
                                    await GetProfilePhotoInByte(id),
                                    await or.GetOrdersByUserId(id),
                                    await cl.GetAllReportsForUser(id),
                                    r.GetBoolean(8)
                                );

                                usersList.Add(user);
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

            return usersList;
        }
        

    public  async Task<int> MakeUserAdmin(int UserID)
        {
            string ConnectionString = @"server=localhost;user id=root;persistsecurityinfo=True;database=project;password=josh17rog";
            using (MySqlConnection connection = new MySqlConnection(ConnectionString))
            {

                string update = $"UPDATE users SET IsAdmin = @IsAdmin WHERE UserID = {UserID}; ";

                try
                {
                    await connection.OpenAsync();

                    using (MySqlCommand UpdateUserCommand = new MySqlCommand(update, connection))
                    {
                        UpdateUserCommand.Parameters.AddWithValue("@IsAdmin", 1);
                        return await UpdateUserCommand.ExecuteNonQueryAsync();
                    }

                }
                catch (Exception ex)
                {
                    
                    Console.WriteLine(ex.ToString());
                    return -1;
                }
            }
        }

    }
}
