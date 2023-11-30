using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model
{
    public class ReportClean
    {
        public ReportClean(int userid, int trashCanId, int weight)
        {
            Userid = userid;
            TrashCanId = trashCanId;
            this.weight = weight;
            this.date = DateTime.Now;
        }

        public int Userid { get; set; }
        public int TrashCanId { get; set; }
        public int weight { get; set; }
        public DateTime date { get; set; }


    }
}
