using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MySql.Data;
using MySql.Data.MySqlClient;
using Model;
using Dapper;

namespace DataLayer
{
    public class ProductDTO:BaseDTO
    {
        public ProductDTO()
        {
			ProductID = -113414;
			ProductName = null;
			ProductDescription =null;
			ProductPrice = -1;
		}

        public ProductDTO(int productID, string productName, string productDescription, int productPrice)
        {
            ProductID = productID;
            ProductName = productName;
            ProductDescription = productDescription;
            ProductPrice = productPrice;
        }

        public int GetPrice()
        {
            return ProductPrice;
        }

        public static async Task< Product> GetProuctFromIndex(int id)
        {
            string connectionString = @"server=localhost;user id=root;persistsecurityinfo=True;database=project;password=josh17rog";
            int userId = id; // Replace with the UserID you want to use.
            Product p = new Product();
            List<int> productIds = new List<int>();

            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                connection.Open();

                string query = "SELECT *" +
                               "FROM Product P " +
                               $"WHERE P.ProductID = ('{id}')";
                using (MySqlCommand cmd = new MySqlCommand(query, connection))
                {
                    

                    using (MySqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            p = new Product(id, reader.GetString(1), reader.GetString(2), int.Parse(reader.GetString(3)),await GetProductPictureInBytes(id));
                        }
                    }
                }
                return p;
            }


        }

        public static async Task<List<Product>> GetAllProducts()
        {
            List<Product> products = new List<Product>();   
            MySqlConnection c = new MySqlConnection();
            c.ConnectionString = @"server=localhost;user id=root;persistsecurityinfo=True;database=project;password=josh17rog";
            c.Open();
            MySqlCommand cmd = new MySqlCommand();
            cmd.Connection = c;
            cmd.CommandText = $@"SELECT *
                                FROM   product;";
            MySqlDataReader r = cmd.ExecuteReader();
            while (r.Read())
            {
                int id = r.GetInt32(0);
                Product p = new Product(id,r.GetString(1),r.GetString(2),r.GetInt32(3), await ProductDTO.GetProductPictureInBytes(id));
                products.Add(p);
            }
            return products;

        }

        private static async Task<byte[]> GetProductPictureInBytes(int id)
        {
            string query = $"Select ProductPicture From product Where ProductID ={id}";

            using (var connection = new MySqlConnection(@"server=localhost;user id=root;persistsecurityinfo=True;database=project;password=josh17rog"))
            {
                connection.Open();

                return  await connection.QueryFirstOrDefaultAsync<byte[]>(query); ;
            }
        }

        public static async Task AddTocart(int userid,int productid)
        {
                  MySqlConnection connection = new MySqlConnection(@"server=localhost;user id=root;persistsecurityinfo=True;database=project;password=josh17rog");
          MySqlCommand cmd = new MySqlCommand();
            cmd.Connection = connection;
            connection.Open();
            string query = $"INSERT INTO cart (ProductID, UserID, quantity) VALUES ('{productid}', '{userid}','1');";
            cmd.CommandText = query;
            cmd.ExecuteNonQuery();
            connection.Close();
        }
        public int ProductID { get; set; }
        public string ProductName { get; set; }
        public string ProductDescription { get; set; }
        public int ProductPrice { get; set; }

    }
}
