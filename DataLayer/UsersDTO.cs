
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

namespace DataLayer
{
    public class UsersDTO
    {
       
        
           
        
        
        public static async Task<List<Product>>GetCart(int id)
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

		public static async Task<string> Register(string Username,string password,string email,int location,int Id)
		{
            MySqlConnection c = new MySqlConnection();
            c.ConnectionString = @"server=localhost;user id=root;persistsecurityinfo=True;database=project;password=josh17rog";
            c.Open();
            MySqlCommand cmd = new MySqlCommand();
            cmd.Connection = c;
			cmd.CommandText = $"INSERT INTO users (UserID,UserEmail,userPassword,usercoin,UserName,Userxp,UserLocation) VALUES ('{Id}','{email}','{password}','0','{Username}','0','{location}');";
            try
            {
                return (await cmd.ExecuteNonQueryAsync()).ToString();
                
            }
            catch(Exception ex)
            {
                string b = ex.Message;
                return b;
            }
			
        }

		public static async Task<Users> Login(string email,string password)
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
			Users ca = new Users() ;
            if (r.HasRows)
            {
                await r.ReadAsync();
				int id = r.GetInt32(0);
                bool isadmin = false;
                if(r.GetBoolean(8) == true)
                {
                    isadmin = true;
                }
                ca = new Users(id, email, r.GetInt32(3), r.GetString(4), (r.GetInt32(5)), (await LocationsDTO.GetLocationFromPK(r.GetInt32(6))),await UsersDTO.GetCart(id), await UsersDTO.GetProfilePhotoInByte(id),isadmin);
                
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
                ca = new Users(id, r.GetString(1),r.GetInt32(3), r.GetString(4), (r.GetInt32(5)), await LocationsDTO.GetLocationFromPK(r.GetInt32(6)), await UsersDTO.GetCart(id), await UsersDTO.GetProfilePhotoInByte(id),isadmin);

            }
            return ca;
        }

        public static async Task<Tuple<int,int>>GetLevelAndPrecentage(int id)
        {
            MySqlConnection c = new MySqlConnection();
            c.ConnectionString = @"server=localhost;user id=root;persistsecurityinfo=True;database=project;password=josh17rog";
            c.Open();
            MySqlCommand cmd = new MySqlCommand();
            cmd.Connection = c;
            string query = $"Select UserXp From users WHERE UserID={id}";
            cmd.CommandText = query ;
            int xp = 0;
            MySqlDataReader r = cmd.ExecuteReader();
            if (r.HasRows)
            {
                r.Read();
                xp = r.GetInt32(0);
                

            }
            int level = xp / 10;

            int precentage =await  GetLastDigit(level)*10;
            return new Tuple<int,int>(level,precentage);
        }

        private  static async Task<int> GetLastDigit(int number)
        {
            // Ensure the input is a non-negative integer
            if (number < 0)
            {
                throw new   ArgumentException("Input must be a non-negative integer", nameof(number));
            }

            // Extract the last digit
            int lastDigit = number % 10;

            return lastDigit;
        }

        public static async Task UpdatePassword(int id,string password)
        {
            MySqlConnection c = new MySqlConnection();
            c.ConnectionString = @"server=localhost;user id=root;persistsecurityinfo=True;database=project;password=josh17rog";
            c.Open();
            MySqlCommand cmd = new MySqlCommand();
            cmd.Connection = c;
            string query = $"UPDATE users SET UserPassword = '{password}' where UserID={id};";
            cmd.CommandText= query ;
            await cmd.ExecuteNonQueryAsync();
        }

        public static async Task UpdateUserName(int id,string UserName)
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

        public static async Task UpdateUserEmail(int id,string UserEmail)
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
        public static async Task UpdateUserCoin(int id,int Coin)
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

        
        public static async Task DeleteProductFromUserCart(int Userid , int productid)
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
    }
}
