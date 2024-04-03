
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

namespace DataLayer
{
    public class UsersDTO
    {
        public UsersDTO(int userID, string email, string password, int coin, string username, int userxp, int userLocation, byte[] profilePicture, bool isAdmin)
        {
            UserID = userID;
            Email = email;
            Password = password;
            Coin = coin;
            Username = username;
            Userxp = userxp;
            UserLocation = userLocation;
            ProfilePicture = profilePicture;
            IsAdmin = isAdmin;
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
            IsAdmin = false;
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
            IsAdmin = U.IsAdmin;
        }

        public UsersDTO()
        {

        }
        public int UserID { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public int Coin { get; set; }
        public string Username { get; set; }
        public int Userxp { get; set; }
        public int UserLocation { get; set; }
        public byte[] ProfilePicture { get; set; }

        public bool IsAdmin { get; set; }


        public static async Task<List<Users>> GetAllUsers()
        {
            List<Users> usersList = new List<Users>();

            // Use a using statement to ensure proper disposal of resources
            using (MySqlConnection c = new MySqlConnection())
            {
                try
                {
                    c.ConnectionString = @"server=localhost;user id=root;persistsecurityinfo=True;database=project;password=josh17rog";
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

                                bool isAdmin = r.GetInt32(8) == 1;

                                // Fetch related data asynchronously using separate methods
                                Users user = new Users(
                                    id,
                                    r.GetString(1),
                                    r.GetInt32(3),
                                    r.GetString(4),
                                    r.GetInt32(5),
                                    await LocationsDTO.GetLocationFromPK(r.GetInt32(6)),
                                    await UsersDTO.GetCart(id),
                                    await UsersDTO.GetProfilePhotoInByte(id),
                                    isAdmin,
                                    await OrderDTO.GetOrdersByUserId(id),
                                    await ReportCleanDTO.GellAllReports(id)
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

        public static async Task<List<Product>> GetCart(int id)
        {




            string connectionString = @"server=localhost;user id=root;persistsecurityinfo=True;database=project;password=josh17rog";
            int userId = id; // Replace with the UserID you want to use.

            List<Product> productIds = new List<Product>();

            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                connection.Open();

                string query = "SELECT P.ProductID " +
                               "FROM Product P " +
                               "JOIN Cart C ON P.ProductID = C.ProductID " +
                               "WHERE C.UserID = @UserID";

                using (MySqlCommand cmd = new MySqlCommand(query, connection))
                {
                    cmd.Parameters.AddWithValue("@UserID", userId);
                    try
                    {
                        using (MySqlDataReader reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                Product productId = await ProductDTO.GetProuctFromIndex(reader.GetInt32("ProductID"));
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

        public static async Task<string> Register(UsersDTO user)
        {
            string connectionString = @"server=localhost;user id=root;persistsecurityinfo=True;database=project;password=josh17rog";

            try
            {
                using (MySqlConnection connection = new MySqlConnection(connectionString))
                {
                    await connection.OpenAsync();

                    string query = @"INSERT INTO users (UserEmail, userPassword, usercoin, UserName, Userxp, UserLocation, ProfilePicture, IsAdmin) 
                             VALUES (@UserEmail, @userPassword, @usercoin, @UserName, @Userxp, @UserLocation, @ProfilePicture, @IsAdmin)";

                    using (MySqlCommand cmd = new MySqlCommand(query, connection))
                    {
                        cmd.Parameters.AddWithValue("@UserEmail", user.Email);
                        cmd.Parameters.AddWithValue("@userPassword", user.Password);
                        cmd.Parameters.AddWithValue("@usercoin", 0); // Assuming usercoin should be defaulted to 0
                        cmd.Parameters.AddWithValue("@UserName", user.Username);
                        cmd.Parameters.AddWithValue("@Userxp", 0); // Assuming Userxp should be defaulted to 0
                        cmd.Parameters.AddWithValue("@UserLocation", user.UserLocation);
                        cmd.Parameters.AddWithValue("@ProfilePicture", user.ProfilePicture);
                        cmd.Parameters.AddWithValue("@IsAdmin", 0); // Assuming IsAdmin should be defaulted to 0

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

        public static async Task<Users> Login(string email, string password)
        {
            MySqlConnection c = new MySqlConnection();
            c.ConnectionString = @"server=localhost;user id=root;persistsecurityinfo=True;database=project;password=josh17rog";
            c.Open();
            MySqlCommand cmd = new MySqlCommand();
            cmd.Connection = c;
            cmd.CommandText = $@"SELECT *
                                FROM   users
                                WHERE  (UserPassword = '{password}') AND
 				 (UserEmail = '{email}')";
            MySqlDataReader r = cmd.ExecuteReader();
            Users ca = new Users();
            if (r.HasRows)
            {
                await r.ReadAsync();
                int id = r.GetInt32(0);
                bool isadmin = false;
                if (r.GetBoolean(8) == true)
                {
                    isadmin = true;
                }
                ca = new Users(id, email, r.GetInt32(3), r.GetString(4), (r.GetInt32(5)), (await LocationsDTO.GetLocationFromPK(r.GetInt32(6))), await UsersDTO.GetCart(id), await UsersDTO.GetProfilePhotoInByte(id), isadmin,await OrderDTO.GetOrdersByUserId(id),await ReportCleanDTO.GellAllReports(id));

            }
            return ca;

        }

        public static async Task<string> GetProfilePhoto(int id)
        {
            string query = $"Select ProfilePicture From users Where UserID ={id}";

            using (var connection = new MySqlConnection(@"server=localhost;user id=root;persistsecurityinfo=True;database=project;password=josh17rog"))
            {
                connection.Open();
                var result = connection.QueryFirstOrDefault<byte[]>(query);
                return $"\"{Convert.ToBase64String(result)}\"";
            }
        }
        public static async Task<byte[]> GetProfilePhotoInByte(int id)
        {
            string query = $"Select ProfilePicture From users Where UserID ={id}";

            using (var connection = new MySqlConnection(@"server=localhost;user id=root;persistsecurityinfo=True;database=project;password=josh17rog"))
            {
                connection.Open();
                var result = connection.QueryFirstOrDefault<byte[]>(query);
                return result;
            }
        }

        public static async Task<Users> GetUserByID(int id)
        {
            MySqlConnection c = new MySqlConnection();
            c.ConnectionString = @"server=localhost;user id=root;persistsecurityinfo=True;database=project;password=josh17rog";
            c.Open();
            MySqlCommand cmd = new MySqlCommand();
            cmd.Connection = c;
            cmd.CommandText = $@"SELECT *
                                FROM   users
                                WHERE  UserID ='{id}'";
            MySqlDataReader r = cmd.ExecuteReader();
            Users ca = new Users();
            if (r.HasRows)
            {
                r.Read();
                bool isadmin = false;
                if (r.GetInt32(8) == 1)
                {
                    isadmin = true;
                }
                ca = new Users(id, r.GetString(1), r.GetInt32(3), r.GetString(4), (r.GetInt32(5)), await LocationsDTO.GetLocationFromPK(r.GetInt32(6)), await UsersDTO.GetCart(id), await UsersDTO.GetProfilePhotoInByte(id), isadmin, await OrderDTO.GetOrdersByUserId(id),await ReportCleanDTO.GellAllReports(id));

            }
            return ca;
        }

        public static async Task<Tuple<int, int>> GetLevelAndPrecentage(int id)
        {
            MySqlConnection c = new MySqlConnection();
            c.ConnectionString = @"server=localhost;user id=root;persistsecurityinfo=True;database=project;password=josh17rog";
            c.Open();
            MySqlCommand cmd = new MySqlCommand();
            cmd.Connection = c;
            string query = $"Select UserXp From users WHERE UserID={id}";
            cmd.CommandText = query;
            int xp = 0;
            MySqlDataReader r = cmd.ExecuteReader();
            if (r.HasRows)
            {
                r.Read();
                xp = r.GetInt32(0);


            }
            int level = xp / 10;

            int precentage = await GetLastDigit(level) * 10;
            return new Tuple<int, int>(level, precentage);
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

        public static async Task UpdateUser(UsersDTO u)
        {
            string ConnectionString = @"server=localhost;user id=root;persistsecurityinfo=True;database=project;password=josh17rog";
            using (MySqlConnection connection = new MySqlConnection(ConnectionString))
            {
                
                string update  = $"UPDATE users SET UserEmail = @UserEmail, Usercoin = @Usercoin, UserName = @UserName, Userxp = @Userxp, UserLocation = @UserLocation, ProfilePicture = @ProfilePicture, IsAdmin = @IsAdmin WHERE UserID = { u.UserID }; ";

                try
                {
                    await connection.OpenAsync();

                    using (MySqlCommand UpdateUserCommand = new MySqlCommand(update, connection))
                    {
                        UpdateUserCommand.Parameters.AddWithValue("@UserEmail",u.Email);
                        UpdateUserCommand.Parameters.AddWithValue("@Usercoin", u.Coin);
                        UpdateUserCommand.Parameters.AddWithValue("@UserName", u.Username);
                        UpdateUserCommand.Parameters.AddWithValue("@Userxp", u.Userxp);
                        UpdateUserCommand.Parameters.AddWithValue("@UserLocation", u.UserLocation);
                        UpdateUserCommand.Parameters.AddWithValue("@ProfilePicture", u.ProfilePicture);
                        UpdateUserCommand.Parameters.AddWithValue("@IsAdmin", u.IsAdmin);

                        await UpdateUserCommand.ExecuteNonQueryAsync();
                    }

                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.ToString());
                }
            }
        }
        public static async Task UpdatePassword(int id, string password)
        {
            MySqlConnection c = new MySqlConnection();
            c.ConnectionString = @"server=localhost;user id=root;persistsecurityinfo=True;database=project;password=josh17rog";
            c.Open();
            MySqlCommand cmd = new MySqlCommand();
            cmd.Connection = c;
            string query = $"UPDATE users SET UserPassword = '{password}' where UserID={id};";
            cmd.CommandText = query;
            await cmd.ExecuteNonQueryAsync();
        }

        

        public static async Task DeleteCart(int Userid)
        {
            MySqlConnection c = new MySqlConnection();
            c.ConnectionString = @"server=localhost;user id=root;persistsecurityinfo=True;database=project;password=josh17rog";
            c.Open();
            MySqlCommand cmd = new MySqlCommand();
            cmd.Connection = c;
            string query = $"Delete from cart where userID = '{Userid}'";
            cmd.CommandText = query;
            await cmd.ExecuteNonQueryAsync();
        }


        public static async Task DeleteProductFromUserCart(int Userid, int productid)
        {
            MySqlConnection c = new MySqlConnection();
            c.ConnectionString = @"server=localhost;user id=root;persistsecurityinfo=True;database=project;password=josh17rog";
            c.Open();
            MySqlCommand cmd = new MySqlCommand();
            cmd.Connection = c;
            string query = $"Delete from cart where userID = '{Userid}' And ProductID = '{productid}' ";
            cmd.CommandText = query;
            await cmd.ExecuteNonQueryAsync();
        }

        public static async Task AddNewOrder(int UserID, List<Product> products, DateTime Date)
        {

            string ConnectionString = @"server=localhost;user id=root;persistsecurityinfo=True;database=project;password=josh17rog";
            using (MySqlConnection connection = new MySqlConnection(ConnectionString))
            {
                // Step 1: Insert a new order into the "order" table
                string insertOrderQuery = $"INSERT INTO `order` (OrderDate, UserID) VALUES (@Date, @UserID);";

                try
                {
                    await connection.OpenAsync();

                    using (MySqlCommand insertOrderCommand = new MySqlCommand(insertOrderQuery, connection))
                    {
                        insertOrderCommand.Parameters.AddWithValue("@Date", Date);
                        insertOrderCommand.Parameters.AddWithValue("@UserID", UserID);

                        await insertOrderCommand.ExecuteNonQueryAsync();
                    }

                    // Step 2: Retrieve the auto-generated OrderID after insertion
                    string getLastInsertedIdQuery = "SELECT LAST_INSERT_ID();";
                    ulong lastInsertedOrderId;

                    using (MySqlCommand getLastInsertedIdCommand = new MySqlCommand(getLastInsertedIdQuery, connection))
                    {
                        lastInsertedOrderId = (ulong)await getLastInsertedIdCommand.ExecuteScalarAsync();
                    }

                    // Step 3: Use the retrieved OrderID to insert records into the "order_product" table
                    await AddProductsToOrder((int)lastInsertedOrderId, products,UserID);
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.ToString());
                }
            }
        }


        private static async Task AddProductsToOrder(int OrderID, List<Product> products, int UserID)
        {

            string ConnectionString = @"server=localhost;user id=root;persistsecurityinfo=True;database=project;password=josh17rog";
            int q = 1;
            try
            {
                using (MySqlConnection connection = new MySqlConnection(ConnectionString))
                {
                    await connection.OpenAsync();

                    foreach (Product product in products)
                    {
                        string insertProductQuery = $"INSERT INTO order_product (Order_ID, ProductID,quantity) VALUES (@OrderID, @ProductID,@q);";

                        using (MySqlCommand insertProductCommand = new MySqlCommand(insertProductQuery, connection))
                        {
                            insertProductCommand.Parameters.AddWithValue("@OrderID", OrderID);
                            insertProductCommand.Parameters.AddWithValue("@ProductID", product.ProductID);
                            insertProductCommand.Parameters.AddWithValue("@q", q);

                            await insertProductCommand.ExecuteNonQueryAsync();
                        }
                    }
                }
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
           
        }
    }
}


