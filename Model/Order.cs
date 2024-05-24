using MySqlX.XDevAPI.Relational;
using System;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model
{
    public class Order
    {
		public Order(int orderID, int userID, DateTime date, List<Product> Products)
        {
			OrderID = orderID;
			UserID = userID;
			Date = DateTime.Now;
			this.Products = Products;
		}

		public Order()
		{
            OrderID = -1;
            UserID=-1;
			Date =DateTime.Now ;
			Products = new List<Product>();
		}

		public int OrderID { get; set; }
        public int UserID { get; set; }

        public DateTime  Date { get; set; }

        public List<Product> Products { get; set; }	



    }
}
