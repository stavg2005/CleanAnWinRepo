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

        public Byte[] profile { get; set; }

        public bool IsAdmin { get; set; }

        public Users(string UserEmail, int coins, string UserName, int xp, Locations location,List<Product> l,bool isadmin)
        {
            this.UserName = UserName;
            Email = UserEmail;
            this.location = location;
            this.xp = xp;
            this.coins = coins;
            products = l;
            profile = null;
            IsAdmin = isadmin;
        }

        public Users(int userID, string UserEmail, int coins, string UserName, int xp, Locations location, List<Product> l, byte[] profile, bool isadmin)
        {
            UserID = userID;
            this.UserName = UserName;
            Email = UserEmail;
            this.location = location;
            this.xp = xp;
            this.coins = coins;
            products = l;
            this.profile = profile;
            IsAdmin = isadmin;
        }

        public Users(string UserEmail, int coins, string UserName, int xp, Locations location, byte[] profile, bool isadmin)
        {
            this.UserName = UserName;
            Email = UserEmail;
            this.location = location;
            this.xp = xp;
            this.coins = coins;
            products = new List<Product>(); ;
            this.profile = profile;
            IsAdmin = isadmin;
        }

        public Users(int id ,string UserEmail,string UserName,Locations location)
        {
            UserID = id;
            UserName = UserName;
            Email = UserEmail;
            location = location;
            this.xp = 0;
            this.coins = 0;
            products = new List<Product>();
            IsAdmin = false;
        }

        public Users()
        {
			UserID = -1;
			UserName = null;
			Email = null;
			location = new Locations();
			this.xp = -1;
			this.coins = -1;
            products = null;
            IsAdmin = false;
		}

        public void Dispose()
        {
            throw new NotImplementedException();
        }
    }
}
