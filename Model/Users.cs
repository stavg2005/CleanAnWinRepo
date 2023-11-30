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
        public string Password { get; set; } 
        public string Email { get; set; }
        public int location { get; set; }

        public int xp { get; set; }

        public int coins { get; set; }

        public List<Product> products { get; set; }

        public Byte[] profile { get; set; }

        public Users(int userID, string UserEmail, string password, int coins, string UserName, int xp, int location,List<Product> l)
        {
            UserID = userID;
            this.UserName = UserName;
            Password = password;
            Email = UserEmail;
            this.location = location;
            this.xp = xp;
            this.coins = coins;
            products = l;
            profile = null;
        }

        public Users(int userID, string UserEmail, string password, int coins, string UserName, int xp, int location, List<Product> l, byte[] profile)
        {
            UserID = userID;
            this.UserName = UserName;
            Password = password;
            Email = UserEmail;
            this.location = location;
            this.xp = xp;
            this.coins = coins;
            products = l;
            this.profile = profile;
        }

        public Users()
        {
			UserID = -1;
			UserName = null;
			Password = null;
			Email = null;
			location = -1;
			this.xp = -1;
			this.coins = -1;
            products = null;
		}

        public void Dispose()
        {
            throw new NotImplementedException();
        }
    }
}
