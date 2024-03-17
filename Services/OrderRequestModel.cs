using Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services
{
    public class OrderRequestModel
    {
        public OrderRequestModel(int userID, int orderID, List<Product> products)
        {
            UserID = userID;
            OrderID = orderID;
            Products = products;
        }

        public int UserID { get; set; }
        public int OrderID { get; set; }
        public List<Product> Products { get; set; } 
    }
}
