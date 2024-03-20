using System;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Model;
using MySqlConnector;
using Dapper;
using System.Linq.Expressions;

namespace DataLayer
{
    public class OrderDTO
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

        private static async Task<List<Product>> GetAllPorudctFromOrder(int orderID)
        {
            List<Product> productList = new List<Product>();
            try
            {
                
                string connectionString = @"server=localhost;user id=root;persistsecurityinfo=True;database=project;password=josh17rog";

                using (MySqlConnection connection = new MySqlConnection(connectionString))
                {
                    string sqlQuery = "SELECT p.ProductID, p.ProductName, p.ProductDes, p.ProductPrice " +
                                      "FROM Product p " +
                                      "INNER JOIN Order_product o ON p.ProductID = o.ProductID " +
                                      "WHERE o.Order_ID = @OrderId";

                    using (MySqlCommand command = new MySqlCommand(sqlQuery, connection))
                    {
                        command.Parameters.AddWithValue("@OrderId", orderID);
                        connection.Open();
                        MySqlDataReader reader = command.ExecuteReader();

                        while (reader.Read())
                        {
                            Product product = new Product
                            {
                                ProductID = Convert.ToInt32(reader["ProductID"]),
                                ProductName = Convert.ToString(reader["ProductName"]),
                                ProductDescription = Convert.ToString(reader["ProductDes"]),
                                ProductPrice = Convert.ToInt32(reader["ProductPrice"])
                            };

                            productList.Add(product);
                        }

                        reader.Close();
                    }
                }
               
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

            return productList;
        }


        public static async Task<List<Order>> GetOrdersByUserId(int userId)
        {
            List<Order> orderList = new List<Order>();
            string connectionString = @"server=localhost;user id=root;persistsecurityinfo=True;database=project;password=josh17rog";
            try
            {
                using (MySqlConnection connection = new MySqlConnection(connectionString))
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
                                OrderID = Convert.ToInt32(reader["OrderID"]),
                                UserID = Convert.ToInt32(reader["UserID"]),
                                Date = (Convert.ToDateTime(reader["OrderDate"])).ToString(),
                                Products = await GetAllPorudctFromOrder(Convert.ToInt32(reader["OrderID"]))
                            };

                            orderList.Add(order);
                        }

                        reader.Close();
                    }
                }

                return orderList;
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.Message);
                return new List<Order>();
            }
           
        }

    }

}
