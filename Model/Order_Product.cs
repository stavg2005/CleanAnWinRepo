using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model
{
	public class Order_Product
	{
		public Order_Product(int orderId, int productId, int quantity)
		{
			OrderId = orderId;
			ProductId = productId;
			this.quantity = quantity;
		}

		public Order_Product()
		{
			OrderId = -1;
			ProductId = -1;
			this.quantity = 0;
		}

		public int OrderId { get; set; }
		public int ProductId { get; set; }

		public int quantity { get; set; }


	}
}
