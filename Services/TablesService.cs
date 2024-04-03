using Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services
{
    public class TablesService
    {
        public TablesService(List<Users> users, List<Locations> locations, List<Order> orders, List<Product> products, List<TrashCan> trashs)
        {
            Users = users;
            Locations = locations;
            Orders = orders;
            Products = products;
            Trashs = trashs;
        }


        public TablesService() { }

        public List<Users> Users { get; set; }
        public List<Locations> Locations { get; set; }

        public List<Order> Orders { get; set; }

        public List<Product> Products { get; set; }

        public List<TrashCan> Trashs { get; set; }
    }
}
