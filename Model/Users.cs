using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model
{
    public class Users:IDisposable
    {
        public int UserID { get; set; }
        public string UserName { get; set; }
        public string Email { get; set; }
        public Locations location { get; set; }

        public int xp { get; set; }

        public int coins { get; set; }

        public List<Product> products { get; set; }

        public List<Order> orders { get; set; }

        public Byte[] profile { get; set; }


        public List<ReportClean> reportCleans { get; set; }

        public int TotalKg { get;set; }

        public bool IsAdmin { get; set; }

        // constructor withouth  profile picture
        public Users(string UserEmail, int coins, string UserName, int xp, Locations location,List<Product> l, List<Order> orders,bool IsAdmin)
        {
            this.UserName = UserName;
            Email = UserEmail;
            this.location = location;
            this.xp = xp;
            this.coins = coins;
            products = l;
            profile = null;
            this.IsAdmin = IsAdmin;
            this.orders = orders;
        }

        // constructor with everything
        public Users(int userID, string UserEmail, int coins, string UserName, int xp, Locations location, List<Product> l, byte[] profile,  List<Order> orders, List<ReportClean> reportClean,bool IsAdmin)
        {
            UserID = userID;
            this.UserName = UserName;
            Email = UserEmail;
            this.location = location;
            this.xp = xp;
            this.coins = coins;
            products = l;
            this.profile = profile;
            this.orders = orders;
            this.reportCleans = reportClean;
            this.TotalKg = GetTotalKG(reportClean);
            this.IsAdmin = IsAdmin;
        }


        //constructor without UserID
        public Users(string UserEmail, int coins, string UserName, int xp, Locations location, byte[] profile, List<Order> orders, bool IsAdmin)
        {
            this.UserName = UserName;
            Email = UserEmail;
            this.location = location;
            this.xp = xp;
            this.coins = coins;
            products = new List<Product>(); ;
            this.profile = profile;
            this.orders = orders;
            this.IsAdmin = IsAdmin;
        }

        

        //constructor for new user 
        public Users(int id, string UserEmail, string UserName, Locations location)
        {
            UserID = id;
            this.UserName = UserName;
            Email = UserEmail;
            this.location = location;
            this.xp = 0;
            this.coins = 0;
            products = new List<Product>();
            this.orders = new List<Order>(0);
            reportCleans = new List<ReportClean>();
            IsAdmin = false;
        }

       // constructor for temp user 
        public Users()
        {
			UserID = -1;
			UserName = null;
			Email = null;
			location = new Locations();
			this.xp = -1;
			this.coins = -1;
            products = null;
            IsAdmin=false;

        }

        //constructor for copying exsisting user
        public Users(Users u)
        {
            UserID = u.UserID;
            UserName = u.UserName;
            Email = u.Email;
            location = u.location;
            this.xp = u.xp;
            this.coins = u.coins;
            this.products = u.products;
            this.profile = u.profile;
            this.orders = u.orders;
            this.IsAdmin = u.IsAdmin;

        }
        private int GetTotalKG(List<ReportClean> reportClean)
        {
            int totalKG = 0;
            foreach( ReportClean r in reportClean)
            { 
                totalKG = TotalKg +r.weight;
            }
            return totalKG;
        }
        public void Dispose()
        {
            throw new NotImplementedException();
        }
    }
}
