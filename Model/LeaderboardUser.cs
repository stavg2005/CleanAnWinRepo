using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model
{
    public class LeaderboardUser
    {
        public string Name { get; set; }
        public int KgCleaned { get; set; }

        public LeaderboardUser() { }
        public LeaderboardUser(string name,int kg)
        { 
            Name = name;
            KgCleaned = kg;
        }
    }
}
