using Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services
{
    public class LeaderBoardService
    {
        public List<LeaderboardUser> TopUsers { get; set; }
        public List<LeaderboardUser> TopUsersThisWeek { get; set; }
        public List<LeaderboardUser> TopUsersToday { get; set; }

        public LeaderBoardService() { }
    }
}
