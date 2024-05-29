using System;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Model;
using MySqlConnector;
using Dapper;
using System.Linq.Expressions;
using System.Configuration;

namespace DataLayer
{
    public class OrderDTO:BaseDTO
    {
        public OrderDTO(int orderID, int userID, string date)
        {
            OrderID = orderID;
            UserID = userID;
            Date = date;
        }

        public OrderDTO()
        {
            OrderID = -1;
            UserID = -1;
            Date = null;
        }

        public int OrderID { get; set; }
        public int UserID { get; set; }

        public string Date { get; set; }

        private  async Task<List<Product>> GetAllPorudctFromOrder(int orderID)
        {
            List<Product> productList = new List<Product>();
            try
            {
                using (MySqlConnection connection = new MySqlConnection(_connectionString))
                {
                    string sqlQuery = @"SELECT p.ProductID, p.ProductName, p.ProductDes, p.ProductPrice 
                                FROM Product p 
                                INNER JOIN Order_product o ON p.ProductID = o.ProductID 
                                WHERE o.Order_ID = @OrderId";

                    using (MySqlCommand command = new MySqlCommand(sqlQuery, connection))
                    {
                        command.Parameters.AddWithValue("@OrderId", orderID);
                        await connection.OpenAsync();

                        using (MySqlDataReader reader = await command.ExecuteReaderAsync())
                        {
                            while (await reader.ReadAsync())
                            {
                                Product product = new Product
                                {
                                    ProductID = reader.GetInt32("ProductID"),
                                    ProductName = reader.GetString("ProductName"),
                                    ProductDescription = reader.GetString("ProductDes"),
                                    ProductPrice = reader.GetInt32("ProductPrice")
                                };

                                productList.Add(product);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
     
            }

            return productList;
        }

        public override async Task<string> Update(object o)
        {
            Order or = (Order)o;
            using (MySqlConnection connection = new MySqlConnection(_connectionString))
            {

                string update = $"UPDATE Order SET  OrderDate = @date, UserID=@ID WHERE OrderID = {or.OrderID}; ";

                try
                {
                    await connection.OpenAsync();

                    using (MySqlCommand UpdateUserCommand = new MySqlCommand(update, connection))
                    {
                        UpdateUserCommand.Parameters.AddWithValue("@date", or.Date);
                        UpdateUserCommand.Parameters.AddWithValue("@UserID", or.UserID);

                        await UpdateUserCommand.ExecuteNonQueryAsync();
                    }
                    return "Success";
                    
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.ToString());
                    return ex.Message;
                }
            }
            
        }
        public override async Task  Delete(int id)
        {
            try
            {
                using (MySqlConnection connection = new MySqlConnection(_connectionString))
                {
                    connection.Open();

                    string query = $"Delete from order where OrderID ={id};";

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
        public  async Task<List<Order>> GetOrdersByUserId(int userId)
        {
            List<Order> orderList = new List<Order>();
            try
            {
                using (MySqlConnection connection = new MySqlConnection(_connectionString))
                {
                    string sqlQuery = "SELECT OrderID, UserID, OrderDate FROM `Order` WHERE UserID = @UserId";

                    using (MySqlCommand command = new MySqlCommand(sqlQuery, connection))
                    {
                        command.Parameters.AddWithValue("@UserId", userId);
                        connection.Open();
                        MySqlDataReader reader = command.ExecuteReader();

                        while (reader.Read())
                        {
                            Order order = new Order
                            {
                                OrderID = reader.GetInt32(0),
                                UserID = userId,
                                Date = reader.GetDateTime(1),
                                Products = await GetAllPorudctFromOrder(reader.GetInt32(0))
                            };

                            orderList.Add(order);
                        }

                        reader.Close();
                    }
                }

                return orderList;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return new List<Order>();
            }

        }

        public override async Task<Order> GetByPK(int orderid)
        {
            try
            {
                using (MySqlConnection connection = new MySqlConnection(_connectionString))
                {
                    string sqlQuery = "SELECT * FROM `Order` WHERE OrderID = @orderid";

                    using (MySqlCommand command = new MySqlCommand(sqlQuery, connection))
                    {
                        command.Parameters.AddWithValue("@UserId", orderid);
                        connection.Open();
                        MySqlDataReader reader = command.ExecuteReader();

                        while (reader.Read())
                        {
                            Order order = new Order
                            {
                                OrderID = orderid,
                                UserID = reader.GetInt32(2),
                                Date = reader.GetDateTime(1),
                                Products = await GetAllPorudctFromOrder(orderid)
                            };
                            return order;
                            
                        }

                        reader.Close();
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

        public override  async Task<List<Order>> SelectAll()
        {
            List<Order> orderList = new List<Order>();
            try
            {
                using (MySqlConnection connection = new MySqlConnection(_connectionString))
                {
                    string sqlQuery = "SELECT * FROM `Order`";

                    using (MySqlCommand command = new MySqlCommand(sqlQuery, connection))
                    {
                        connection.Open();
                        MySqlDataReader reader = command.ExecuteReader();

                        while (reader.Read())
                        {
                            Order order = await GetByPK(reader.GetInt32(0));


                            orderList.Add(order);
                        }

                        reader.Close();
                    }
                }

                return orderList;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return new List<Order>();
            }

        }

        public override async Task Insert(object o)
        {
            Order or = (Order)o;
            await AddNewOrder(or.UserID, or.Products, or.Date);
        }

        private async Task AddProductsToOrder(int OrderID, List<Product> products, int UserID)
        {
            int q = 1;
            try
            {
                using (MySqlConnection connection = new MySqlConnection(_connectionString))
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
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }

        }
        public async Task AddNewOrder(int UserID, List<Product> products, DateTime Date)
        {

            
            using (MySqlConnection connection = new MySqlConnection(_connectionString))
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
                    int lastInsertedOrderId;

                    using (MySqlCommand getLastInsertedIdCommand = new MySqlCommand(getLastInsertedIdQuery, connection))
                    {
                        lastInsertedOrderId = Convert.ToInt32(await getLastInsertedIdCommand.ExecuteScalarAsync());
                    }

                    // Step 3: Use the retrieved OrderID to insert records into the "order_product" table
                    await AddProductsToOrder(lastInsertedOrderId, products, UserID);
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.ToString());
                }
            }
        }
    }

        
}

       
  


