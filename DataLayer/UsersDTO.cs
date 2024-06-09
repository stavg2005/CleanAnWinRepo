
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Model;
using System;
using System.Collections.Generic;
using MySqlX.XDevAPI;
using static System.Runtime.InteropServices.JavaScript.JSType;
using System.Drawing.Imaging;
using Dapper;
using MySqlConnector;
using Org.BouncyCastle.X509;
using System.Diagnostics;
using System.Data.SqlTypes;


namespace DataLayer
{
    public class UsersDTO:BaseDTO
    {
        public UsersDTO(int userID, string email, string password, int coin, string username, int userxp, int userLocation, byte[] profilePicture)
        {
            UserID = userID;
            Email = email;
            Password = password;
            Coin = coin;
            Username = username;
            Userxp = userxp;
            UserLocation = userLocation;
            ProfilePicture = profilePicture;

        }

        public UsersDTO(string email, string password, string username, int userLocation, byte[] profilePicture)
        {
            UserID = -1;
            Email = email;
            Password = password;
            Coin = 0;
            Username = username;
            Userxp = 0;
            UserLocation = userLocation;
            ProfilePicture = profilePicture;

        }

        public UsersDTO(Users U)
        {
            UserID = U.UserID;
            Email = U.Email;
            Coin = U.coins;
            Username = U.UserName;
            Password = "";
            Userxp = U.xp;
            UserLocation = U.location.ID;
            ProfilePicture = U.profile;

        }


        public int UserID { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public int Coin { get; set; }
        public string Username { get; set; }
        public int Userxp { get; set; }
        public int UserLocation { get; set; }
        public byte[] ProfilePicture { get; set; }
        protected LocationsDTO l = new LocationsDTO();
        protected ReportCleanDTO cl = new ReportCleanDTO();
        protected OrderDTO or = new OrderDTO();

        public UsersDTO()
        {
            

            
        }





        public override  async Task<List<Users>> SelectAll()
        {
            List<Users> usersList = new List<Users>();

            // Use a using statement to ensure proper disposal of resources
            using (MySqlConnection c = new MySqlConnection())
            {
                try
                {
                    c.ConnectionString = _connectionString;
                    await c.OpenAsync(); // Open the connection asynchronously

                    string query = @"SELECT * FROM users";
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

        public  async Task<int> GetUserIdFromEmail(string email)
        {
            using (MySqlConnection c = new MySqlConnection())
            {

                int id = -1;
                try
                {
                    c.ConnectionString = _connectionString;
                    await c.OpenAsync(); // Open the connection asynchronously

                    string query = $@"SELECT UserID FROM users WHERE UserEmail= '{email}'";
                    using (MySqlCommand cmd = new MySqlCommand(query, c))
                    {
                        // Execute the query asynchronously
                        using (MySqlDataReader r = await cmd.ExecuteReaderAsync())
                        {
                            while (await r.ReadAsync())
                            {
                                id = r.GetInt32(0); // Assuming UserID is at index 0 in the result set




                            }
                        }
                    }
                    return id;
                }
                catch (Exception ex)
                {
                    // Handle exceptions appropriately (e.g., log, rethrow, etc.)
                    Console.WriteLine($"An error occurred: {ex.Message}");
                    return -1;
                }
            }
        }

        public async Task<List<Product>> GetUserCart(int id)
        { 
            List<Product> productIds = new List<Product>();

            using (MySqlConnection connection = new MySqlConnection(_connectionString))
            {
                connection.Open();

                string query = "SELECT P.ProductID " +
                               "FROM Product P " +
                               "JOIN Cart C ON P.ProductID = C.ProductID " +
                               "WHERE C.UserID = @UserID";

                using (MySqlCommand cmd = new MySqlCommand(query, connection))
                {
                    cmd.Parameters.AddWithValue("@UserID", id);
                    try
                    {
                        using (MySqlDataReader reader = cmd.ExecuteReader())
                        {
                            ProductDTO p = new ProductDTO();
                            while (reader.Read())
                            {
                                Product productId = await p.GetByPK(reader.GetInt32("ProductID"));
                                productIds.Add(productId);
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }

                }
            }

            return productIds;
        }

        public override async Task<string> Insert(object o)
        {
            if (o is UsersDTO)
            {
                UsersDTO user = (UsersDTO)o;
                return await Register(user);
            }
            return "An error has occurred";
        }
        public override async Task<string> Delete(int id)
        {
            try
            {
                using (MySqlConnection connection = new MySqlConnection(_connectionString))
                {
                    connection.Open();

                    string query = $"Delete from users where UserID ={id};";

                    using (MySqlCommand cmd = new MySqlCommand(query, connection))
                    {

                        await cmd.ExecuteNonQueryAsync();

                    }
                    return "Delete Operation Succecfull";

                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return ex.Message;
            }
        }
        public async Task<string> Register(UsersDTO user)
        {
            

            try
            {
                using (MySqlConnection connection = new MySqlConnection(_connectionString))
                {
                    await connection.OpenAsync();

                    string query = @"INSERT INTO users (UserEmail, userPassword, usercoin, UserName, Userxp, UserLocation, ProfilePicture,IsAdmin) 
                             VALUES (@UserEmail, @userPassword, @usercoin, @UserName, @Userxp, @UserLocation, @ProfilePicture,0)";

                    using (MySqlCommand cmd = new MySqlCommand(query, connection))
                    {
                        cmd.Parameters.AddWithValue("@UserEmail", user.Email);
                        cmd.Parameters.AddWithValue("@userPassword", user.Password);
                        cmd.Parameters.AddWithValue("@usercoin", 0); // Assuming usercoin should be defaulted to 0
                        cmd.Parameters.AddWithValue("@UserName", user.Username);
                        cmd.Parameters.AddWithValue("@Userxp", 0); // Assuming Userxp should be defaulted to 0
                        cmd.Parameters.AddWithValue("@UserLocation", user.UserLocation);
                        cmd.Parameters.AddWithValue("@ProfilePicture", user.ProfilePicture);
                        

                        int rowsAffected = await cmd.ExecuteNonQueryAsync();
                        Debug.WriteLine($"{rowsAffected} row(s) affected.");
                        return $"{rowsAffected} row(s) affected.";
                    }
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"An error occurred: {ex.Message}");
                return $"An error occurred: {ex.Message}";
            }
        }

        public async Task<Users> Login(string email, string password)
        {
            Users user = new Users();
            try
            {

            
            // Use a using statement to ensure the connection is disposed
            using (var connection = new MySqlConnection(_connectionString))
            {
                await connection.OpenAsync();

                // Use parameterized queries to prevent SQL injection
                using (var command = new MySqlCommand("SELECT * FROM users WHERE UserEmail = @Email AND UserPassword = @Password", connection))
                {
                    command.Parameters.AddWithValue("@Email", email);
                    command.Parameters.AddWithValue("@Password", password);

                    using (var reader = await command.ExecuteReaderAsync())
                    {
                        if (await reader.ReadAsync())
                        {
                            int id = reader.GetInt32(0);
                            user = new Users(
                                id,
                                email,
                                reader.GetInt32(3),
                                reader.GetString(4),
                                reader.GetInt32(5),
                                await l.GetByPK(reader.GetInt32(6)),
                                await GetUserCart(id),
                                await GetProfilePhotoInByte(id),
                                await or.GetOrdersByUserId(id),
                                await cl.GetAllReportsForUser(id),
                                reader.GetBoolean(8)
                            );
                        }
                    }
                }
                    return user;
              }
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.Message);
                return user;
            }

            

        }

        

        public  async Task<string> GetProfilePhoto(int id)
        {
            

            return $"\"{Convert.ToBase64String(await GetProfilePhotoInByte(id))}\"";
            
        }
        public async Task<byte[]> GetProfilePhotoInByte(int id)
        {
             string query = "SELECT ProfilePicture FROM users WHERE UserID = @UserID";
            try
            {
                using (var connection = new MySqlConnection(_connectionString))
                {
                    await connection.OpenAsync();

                    using (var command = new MySqlCommand(query, connection))
                    {
                        // Use parameterized query to prevent SQL injection
                        command.Parameters.AddWithValue("@UserID", id);

                        using (var reader = await command.ExecuteReaderAsync())
                        {
                            if (await reader.ReadAsync())
                            {
                                return reader["ProfilePicture"] as byte[];
                            }
                        }
                    }
                }
                return null;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return null;
            }
        }

        
        public override async Task<Users> GetByPK(int id)
        {
             string query = "SELECT * FROM users WHERE UserID = @UserId";
            try
            {


                using (var connection = new MySqlConnection(_connectionString))
                {
                    await connection.OpenAsync();

                    using (var command = new MySqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@UserId", id);

                        using (var reader = await command.ExecuteReaderAsync())
                        {
                            if (await reader.ReadAsync())
                            {
                                return new Users(
                                    id,
                                    reader.GetString(1),
                                    reader.GetInt32(3),
                                    reader.GetString(4),
                                    reader.GetInt32(5),
                                    await l.GetByPK(reader.GetInt32(6)),
                                    await GetUserCart(id),
                                    await GetProfilePhotoInByte(id),
                                    await or.GetOrdersByUserId(id),
                                    await cl.GetAllReportsForUser(id),
                                    reader.GetBoolean(8)
                                );
                            }
                        }
                    }
                    return new Users();
                }
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.Message);
                return new Users();
            }
            
        }

        public async Task<Tuple<int, int>> GetLevelAndPrecentage(int id)
        {

            int xp = 0;
            string query = $"Select UserXp From users WHERE UserID=@user id";
            try
            {


                using (var connection = new MySqlConnection(_connectionString))
                {
                    await connection.OpenAsync();
                    using (var command = new MySqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@id", id);
                        using (var reader = await command.ExecuteReaderAsync())
                        {
                            if (reader.HasRows)
                            {
                                reader.Read();
                                xp = reader.GetInt32(0);


                            }
                            int level = xp / 10;

                            int precentage = await GetLastDigit(level) * 10;
                            return new Tuple<int, int>(level, precentage);
                        }
                    }
                }
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.Message);
                return null;
            }
            
        
            
        }

        private static async Task<int> GetLastDigit(int number)
        {
            // Ensure the input is a non-negative integer
            if (number < 0)
            {
                throw new ArgumentException("Input must be a non-negative integer", nameof(number));
            }

            // Extract the last digit
            int lastDigit = number % 10;

            return lastDigit;
        }

        public override  async Task Update(object O)
        {
            if (O is UsersDTO)
            {
                UsersDTO u = (UsersDTO)O;
                using (MySqlConnection connection = new MySqlConnection(_connectionString))
                {

                    string update = $"UPDATE users SET UserEmail = @UserEmail, Usercoin = @Usercoin, UserName = @UserName, Userxp = @Userxp, UserLocation = @UserLocation, ProfilePicture = @ProfilePicture WHERE UserID = {u.UserID}; ";

                    try
                    {
                        await connection.OpenAsync();

                        using (MySqlCommand UpdateUserCommand = new MySqlCommand(update, connection))
                        {
                            UpdateUserCommand.Parameters.AddWithValue("@UserEmail", u.Email);
                            UpdateUserCommand.Parameters.AddWithValue("@Usercoin", u.Coin);
                            UpdateUserCommand.Parameters.AddWithValue("@UserName", u.Username);
                            UpdateUserCommand.Parameters.AddWithValue("@Userxp", u.Userxp);
                            UpdateUserCommand.Parameters.AddWithValue("@UserLocation", u.UserLocation);
                            UpdateUserCommand.Parameters.AddWithValue("@ProfilePicture", u.ProfilePicture);

                             await UpdateUserCommand.ExecuteNonQueryAsync();
                        }
                        
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.ToString());
                    }
                }
            }
           
        }
        public  async Task UpdatePassword(int id, string password)
        {
            try
            {
                string query = "UPDATE users SET UserPassword = @password WHERE UserID = @id";
                using (MySqlConnection connection = new MySqlConnection(@"server=localhost;user id=root;persistsecurityinfo=True;database=project;password=josh17rog"))
                {
                    await connection.OpenAsync();

                    using (MySqlCommand cmd = new MySqlCommand(query,connection))
                    {
                        cmd.Parameters.AddWithValue("@password", password);
                        cmd.Parameters.AddWithValue("@id", id);

                        await cmd.ExecuteNonQueryAsync();
                    }
                }
            }
            catch (Exception ex)
            {
                
                Console.WriteLine("An error occurred: " + ex.Message);
                
            }

          
        }



        public async Task DeleteCart(int UserID)
        {
            try
            {
                string query = "DELETE FROM cart WHERE userID = @userId";
                using (MySqlConnection connection = new MySqlConnection(_connectionString))
                {
                    await connection.OpenAsync();

                    using (MySqlCommand cmd = new MySqlCommand(query,connection))
                    {

                        cmd.Parameters.AddWithValue("@userId", UserID);

                        await cmd.ExecuteNonQueryAsync();
                    }
                }
            }
            catch (Exception ex)
            {
                
                Console.WriteLine("An error occurred: " + ex.Message);
                
            }

            
        }


        public  async Task DeleteProductFromUserCart(int Userid, int productid)
        {
             string query = "DELETE FROM cart WHERE userID = @UserId AND ProductID = @ProductId";

            try
            {
                using (var connection = new MySqlConnection(_connectionString))
                {
                    await connection.OpenAsync();

                    using (var command = new MySqlCommand(query, connection))
                    {
                        // Use parameterized queries to prevent SQL injection
                        command.Parameters.AddWithValue("@UserId", Userid);
                        command.Parameters.AddWithValue("@ProductId", productid);

                        await command.ExecuteNonQueryAsync();
                    }
                }
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        


        

        public  async Task<List<LeaderboardUser>> GetTopUsersToday()
        {
            try
            {
                string _connectionString = @"server=localhost;user id=root;persistsecurityinfo=True;database=project;password=josh17rog";
                // Define the start and end of today
                DateTime startOfToday = DateTime.Today;
                
                DateTime endOfToday = startOfToday.AddDays(1).AddTicks(-1);
                string s = startOfToday.ToString() + "end of day is " + endOfToday.ToString();
                // Query to get top users who cleaned the most today
                string query = @"
            SELECT u.UserName, SUM(cr.Weight) AS TotalWeight
            FROM clean_report cr
            JOIN Users u ON cr.UserId = u.UserId
            WHERE cr.Date >= @startOfToday AND cr.Date <= @endOfToday
            GROUP BY u.UserName
            ORDER BY TotalWeight DESC
            LIMIT 5";

                var topUsersToday = new List<LeaderboardUser>();

                await using (var connection = new MySqlConnection(_connectionString))
                {
                    await connection.OpenAsync();
                    using (var command = new MySqlCommand(query, connection))
                    {
                        // Add parameters to the query
                        command.Parameters.AddWithValue("@startOfToday", startOfToday);
                        command.Parameters.AddWithValue("@endOfToday", endOfToday);

                        using (var reader = await command.ExecuteReaderAsync())
                        {
                            while (await reader.ReadAsync())
                            {
                                var name = reader.GetString(0);
                                var totalWeight = reader.GetDecimal(1);

                                topUsersToday.Add(new LeaderboardUser
                                {
                                    Name = name,
                                    KgCleaned = Convert.ToInt32(totalWeight)
                                });
                            }
                        }
                    }
                }
                return topUsersToday;
            }
            catch(Exception ex)
            { 
                Console.WriteLine(ex.ToString());
                throw new Exception(ex.Message);
            }
            
        }

        public  async Task<List<LeaderboardUser>> GetTopUsersThisWeek()
        {
            try
            {
                string _connectionString = @"server=localhost;user id=root;persistsecurityinfo=True;database=project;password=josh17rog";

                // Define the start of the current week (assuming Monday as the first day of the week)
                DateTime today = DateTime.Today;
                int diff = (7 + (today.DayOfWeek - DayOfWeek.Monday)) % 7;
                DateTime startOfWeek = today.AddDays(-1 * diff).Date;
                DateTime endOfWeek = startOfWeek.AddDays(7).AddTicks(-1);

                // Query to get top users who cleaned the most this week
                string query = @"
        SELECT u.UserName, SUM(cr.Weight) AS TotalWeight
        FROM clean_report cr
        JOIN Users u ON cr.UserId = u.UserId
        WHERE cr.Date >= @startOfWeek AND cr.Date <= @endOfWeek
        GROUP BY u.UserName
        ORDER BY TotalWeight DESC
        LIMIT 5";

                var topUsersThisWeek = new List<LeaderboardUser>();

                await using (var connection = new MySqlConnection(_connectionString))
                {
                    await connection.OpenAsync();
                    using (var command = new MySqlCommand(query, connection))
                    {
                        // Add parameters to the query
                        command.Parameters.AddWithValue("@startOfWeek", startOfWeek);
                        command.Parameters.AddWithValue("@endOfWeek", endOfWeek);

                        using (var reader = await command.ExecuteReaderAsync())
                        {
                            while (await reader.ReadAsync())
                            {
                                var name = reader.GetString(0);
                                var totalWeight = reader.GetDecimal(1);

                                topUsersThisWeek.Add(new LeaderboardUser
                                {
                                    Name = name,
                                    KgCleaned = Convert.ToInt32(totalWeight)
                                });
                            }
                        }
                    }
                }
                return topUsersThisWeek;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error occurred: {ex.Message}");
                throw;
            }

        }

        public async Task<List<LeaderboardUser>> GetTopUsersOfAllTime()
        {
            try
            {
                string _connectionString = @"server=localhost;user id=root;persistsecurityinfo=True;database=project;password=josh17rog";

                // Query to get top users who cleaned the most of all time
                string query = @"
        SELECT u.UserName, SUM(cr.Weight) AS TotalWeight
        FROM clean_report cr
        JOIN Users u ON cr.UserId = u.UserId
        GROUP BY u.UserName
        ORDER BY TotalWeight DESC
        LIMIT 5";

                var topUsersOfAllTime = new List<LeaderboardUser>();

                await using (var connection = new MySqlConnection(_connectionString))
                {
                    await connection.OpenAsync();
                    using (var command = new MySqlCommand(query, connection))
                    {
                        using (var reader = await command.ExecuteReaderAsync())
                        {
                            while (await reader.ReadAsync())
                            {
                                var name = reader.GetString(0);
                                var totalWeight = reader.GetDecimal(1);

                                topUsersOfAllTime.Add(new LeaderboardUser
                                {
                                    Name = name,
                                    KgCleaned = Convert.ToInt32(totalWeight)
                                });
                            }
                        }
                    }
                }
                return topUsersOfAllTime;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error occurred: {ex.Message}");
                throw;
            }
        }
        public async Task<Tuple<int,int>> GetCoinandxpValue()
        {
            try
            {
                using (MySqlConnection c = new MySqlConnection(_connectionString))
                {
                    await c.OpenAsync();

                    string query = @"SELECT * FROM coinandxpvalue WHERE ID = @id";

                    using (MySqlCommand cmd = new MySqlCommand(query, c))
                    {

                        cmd.Parameters.AddWithValue("@id", 1);

                        using (MySqlDataReader r = await cmd.ExecuteReaderAsync())
                        {
                            if (await r.ReadAsync())
                            {
                                return new Tuple<int,int>(r.GetInt32(1),r.GetInt32(2));
                            }
                        }
                         return new Tuple<int, int>(0,0);
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                return new Tuple<int, int>(0, 0); 
            }


        }

        public async Task<bool> CheakIfUserNameExist(string username)
        {
            try
            {
                using(MySqlConnection c = new MySqlConnection(_connectionString))
                {
                    await c.OpenAsync();
                    string query = $"SELECT UserID FROM users WHERE UserName = '{username}'";
                    using (MySqlCommand cmd = new MySqlCommand(query, c))
                    {

                        

                        using (MySqlDataReader r = await cmd.ExecuteReaderAsync())
                        {
                            if (r.HasRows)
                            {
                                return true;
                            }
                        }
                        return false;
                    }
                }
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.ToString());
                return false;
            }
        }

        
    }
}



