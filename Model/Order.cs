using System;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model
{
    public class Order
    {
		public Order(int orderID, int userID, string date)
		{
			OrderID = orderID;
			UserID = userID;
			Date = date;
		}

		public Order()
		{
			OrderID = -1;
			UserID=-1;
			Date = null;
		}

		public int OrderID { get; set; }
        public int UserID { get; set; }
        
        public string Date { get; set; }



    }
}
