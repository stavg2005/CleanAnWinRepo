
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
                ca = new Users(id, email, r.GetInt32(3), r.GetString(4), (r.GetInt32(5)), (await LocationsDTO.GetLocationFromPK(r.GetInt32(6))), await UsersDTO.GetCart(id), await UsersDTO.GetProfilePhotoInByte(id), isadmin,await OrderDTO.GetOrdersByUserId(id));

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
                ca = new Users(id, r.GetString(1), r.GetInt32(3), r.GetString(4), (r.GetInt32(5)), await LocationsDTO.GetLocationFromPK(r.GetInt32(6)), await UsersDTO.GetCart(id), await UsersDTO.GetProfilePhotoInByte(id), isadmin, await OrderDTO.GetOrdersByUserId(id));

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

        public static async Task UpdateUserName(int id, string UserName)
        {
            MySqlConnection c = new MySqlConnection();
            c.ConnectionString = @"server=localhost;user id=root;persistsecurityinfo=True;database=project;password=josh17rog";
            c.Open();
            MySqlCommand cmd = new MySqlCommand();
            cmd.Connection = c;
            string query = $"UPDATE users SET UserName = '{UserName}' where UserID={id};";
            cmd.CommandText = query;
            await cmd.ExecuteNonQueryAsync();
        }

        public static async Task UpdateUserEmail(int id, string UserEmail)
        {
            MySqlConnection c = new MySqlConnection();
            c.ConnectionString = @"server=localhost;user id=root;persistsecurityinfo=True;database=project;password=josh17rog";
            c.Open();
            MySqlCommand cmd = new MySqlCommand();
            cmd.Connection = c;
            string query = $"UPDATE users SET UserEmail = '{UserEmail}' where UserID={id};";
            cmd.CommandText = query;
            await cmd.ExecuteNonQueryAsync();
        }
        public static async Task UpdateUserCoin(int id, int Coin)
        {
            MySqlConnection c = new MySqlConnection();
            c.ConnectionString = @"server=localhost;user id=root;persistsecurityinfo=True;database=project;password=josh17rog";
            c.Open();
            MySqlCommand cmd = new MySqlCommand();
            cmd.Connection = c;
            string query = $"UPDATE users SET UserCoin = '{Coin}' where UserID={id};";
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

        public static async Task AddNewOrder(int OrderID,int Userid, List<Product> products, DateTime Date)
        {

            string ConnectionString = @"server=localhost;user id=root;persistsecurityinfo=True;database=project;password=josh17rog";
            using (MySqlConnection connection = new MySqlConnection(ConnectionString))
            {
                string sqlQuery = $"Insert Into order (OrderDate,UserID) Values ({Date},{Userid});";

                try
                {
                    using (MySqlCommand command = new MySqlCommand(sqlQuery, connection))
                    {

                        connection.Open();
                        command.CommandText = sqlQuery;
                        int i =command.ExecuteNonQuery();
                        if(i > 0)
                        {
                            await AddProductsToOrder(OrderID,products,Userid);
                        }
                    }
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
            using (MySqlConnection connection = new MySqlConnection(ConnectionString))
            {
                foreach (Product p in products)
                {
                    string sqlQuery = $"Insert Into order_Products (OrderDate,ProductID,UserID) Values ({OrderID},{p.ProductID},{UserID});";

                    try
                    {
                        using (MySqlCommand command = new MySqlCommand(sqlQuery, connection))
                        {

                            connection.Open();
                            command.CommandText = sqlQuery;
                            command.ExecuteNonQuery();
                        }
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.ToString());
                    }
                }


            }
        }
    }
}


