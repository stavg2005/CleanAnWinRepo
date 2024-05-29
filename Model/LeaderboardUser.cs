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
        public float KgCleaned { get; set; }

        public LeaderboardUser() { }
        public LeaderboardUser(string name,float kg)
        { 
            Name = name;
            KgCleaned = kg;
        }
    }
}
