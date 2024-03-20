using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model
{
    public class Product
    {
        public Product()
        {
			ProductID = -1;
			ProductName = null;
			ProductDescription =null;
			ProductPrice = -1;
            LevelReq = 0;
		}

        public Product(int productID, string productName, string productDescription, int productPrice, byte[] productPicture)
        {
            ProductID = productID;
            ProductName = productName;
            ProductDescription = productDescription;
            ProductPrice = productPrice;
            ProductPicture = productPicture;
            LevelReq = 0;
        }
        public Product(int productID, string productName, string productDescription, int productPrice, byte[] productPicture,int LevelReq)
        {
            ProductID = productID;
            ProductName = productName;
            ProductDescription = productDescription;
            ProductPrice = productPrice;
            ProductPicture = productPicture;
            this.LevelReq = LevelReq;
        }

        public int GetPrice()
        {
            return ProductPrice;
        }


        public int ProductID { get; set; }
        public string ProductName { get; set; }
        public string ProductDescription { get; set; }
        public int ProductPrice { get; set; }
        public int LevelReq { get; set; }
        public byte[] ProductPicture {  get; set; }

    }
}
