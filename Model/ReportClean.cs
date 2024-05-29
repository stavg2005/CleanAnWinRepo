using Org.BouncyCastle.Bcpg.OpenPgp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model
{
    public class ReportClean
    {
        public ReportClean(int userid, int trashCanId, float weight,int reportid)
        {
            Userid = userid;
            TrashCanId = trashCanId;
            this.weight = weight;
            this.date = DateTime.Now;
            this.ReportID = reportid;

        }

        public ReportClean(int userid, int trashCanId, float weight)
        { 
            Userid = userid;
            TrashCanId = trashCanId;
            this.weight = weight;
            this.date = DateTime.Now;
            this.ReportID = -1;

        }
        public ReportClean(int userid, int trashCanId, float weight, DateTime date, int reportid) : this(userid, trashCanId, weight, reportid)
        {
            this.date = date;
        }
        public ReportClean() { }

        public int ReportID { get; set; }
        public int Userid { get; set; }
        public int TrashCanId { get; set; }
        public float weight { get; set; }
        public DateTime date { get; set; }


    }
}
