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
			ProductID = -1;
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

            

        public  override async Task< Product> GetByPK(int id)
        {
            Product p = new Product();
            List<int> productIds = new List<int>();

            using (MySqlConnection connection = new MySqlConnection(_connectionString))
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
                            p = new Product(id, reader.GetString(1), reader.GetString(2), int.Parse(reader.GetString(3)),await GetProductPictureInBytes(id),reader.GetInt32(5));
                        }
                    }
                }
                return p;
            }


        }

        public override  async Task<List<Product>> SelectAll()
        {

            const string query = "SELECT * FROM product";

            var products = new List<Product>();

            try
            {
                using (var connection = new MySqlConnection(_connectionString))
                {
                    await connection.OpenAsync();

                    using (var command = new MySqlCommand(query, connection))
                    {
                        using (var reader = await command.ExecuteReaderAsync())
                        {
                            while (await reader.ReadAsync())
                            {
                                int id = reader.GetInt32(0);
                                var product = new Product(
                                    id,
                                    reader.GetString(1),
                                    reader.GetString(2),
                                    reader.GetInt32(3),
                                    await GetProductPictureInBytes(id),
                                    reader.GetInt32(5)
                                );
                                products.Add(product);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                
            }

            return products;


        }

        private  async Task<byte[]> GetProductPictureInBytes(int id)
        {
            const string query = "SELECT ProductPicture FROM product WHERE ProductID = @ProductId";

            using (var connection = new MySqlConnection(_connectionString))
            {
                await connection.OpenAsync();

                using (var command = new MySqlCommand(query, connection))
                {
                    // Use parameterized query to prevent SQL injection
                    command.Parameters.AddWithValue("@ProductId", id);

                    using (var reader = await command.ExecuteReaderAsync())
                    {
                        if (await reader.ReadAsync())
                        {
                            return reader["ProductPicture"] as byte[];
                        }
                    }
                }
            }

            // Return null if no picture is found
            return null;
        }

        public async Task AddTocart(int userId, int productId)
        {
            using (MySqlConnection connection = new MySqlConnection(_connectionString))
            {
                try
                {
                    await connection.OpenAsync();

                    string query = "INSERT INTO cart (ProductID, UserID) VALUES (@productId, @userId)";

                    using (MySqlCommand cmd = new MySqlCommand(query, connection))
                    {
                        cmd.Parameters.AddWithValue("@productId", productId);
                        cmd.Parameters.AddWithValue("@userId", userId);

                        await cmd.ExecuteNonQueryAsync();
                    }
                }
                catch (Exception ex)
                {
                    // Handle the exception appropriately
                    Console.WriteLine("An error occurred: " + ex.Message);
                    // You might want to throw or log the exception depending on your requirements
                }
            }
        }

        public override  async Task Update(object o)
        {
            if (o is Product)
            {


                Product P = (Product)o;
                using (MySqlConnection connection = new MySqlConnection(_connectionString))
                {

                    string update = $"UPDATE product SET ProductName = @Name, ProductDes = @Des, ProductPrice = @Price, ProductPicture = @Pic, Levelreq = @req WHERE ProductID = {P.ProductID}; ";

                    try
                    {
                        await connection.OpenAsync();

                        using (MySqlCommand UpdateUserCommand = new MySqlCommand(update, connection))
                        {
                            UpdateUserCommand.Parameters.AddWithValue("@Name", P.ProductName);
                            UpdateUserCommand.Parameters.AddWithValue("@Des", P.ProductDescription);
                            UpdateUserCommand.Parameters.AddWithValue("@Price", P.ProductPrice);
                            UpdateUserCommand.Parameters.AddWithValue("@Pic", P.ProductPicture);
                            UpdateUserCommand.Parameters.AddWithValue("@req", P.LevelReq);

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

        public override  async Task Insert(object o)
        {
            if (o is Product)
            {


                Product p = (Product)o;
                try
                {
                    string connectionString = @"server=localhost;user id=root;persistsecurityinfo=True;database=project;password=josh17rog";

                    using (MySqlConnection connection = new MySqlConnection(connectionString))
                    {
                        await connection.OpenAsync();

                        string query = "INSERT INTO product (ProductName, ProductDes, ProductPrice, ProductPicture) VALUES (@ProductName, @ProductDescription, @ProductPrice, @ProductPicture)";

                        using (MySqlCommand cmd = new MySqlCommand(query, connection))
                        {
                            cmd.Parameters.AddWithValue("@ProductName", p.ProductName);
                            cmd.Parameters.AddWithValue("@ProductDescription", p.ProductDescription);
                            cmd.Parameters.AddWithValue("@ProductPrice", p.ProductPrice);
                            cmd.Parameters.AddWithValue("@ProductPicture", p.ProductPicture);

                             await cmd.ExecuteNonQueryAsync();
                        }
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);

                }
            }

        }

        public override async Task Delete(int id)
        {
            try
            {
                using (MySqlConnection connection = new MySqlConnection(_connectionString))
                {
                    connection.Open();

                    string query = $"Delete from product where ProductID ={id};";

                    using (MySqlCommand cmd = new MySqlCommand(query, connection))
                    {

                         await cmd.ExecuteNonQueryAsync();

                    }
                    

                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
               
            }
        }

        public int ProductID { get; set; }
        public string ProductName { get; set; }
        public string ProductDescription { get; set; }
        public int ProductPrice { get; set; }

    }
}
