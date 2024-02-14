using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Model;

namespace Services
{
    public class ProductService
    {
        public List<Product> ProductList { get; set; }
        public ProductService()
        {
            ProductList = new List<Product>(0);
        }
    }
}
