using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model
{
    public class WeightLog
    {
        public WeightLog(int logId, int trashCanId, float oldWeight, float newWeight, float weightDifference, DateTime changeTime)
        {
            LogId = logId;
            TrashCanId = trashCanId;
            OldWeight = oldWeight;
            NewWeight = newWeight;
            WeightDifference = weightDifference;
            ChangeTime = changeTime;
        }

        public int LogId { get; set; }
        public int TrashCanId { get; set; }
        public float OldWeight { get; set; }
        public float NewWeight { get; set; }
        public float WeightDifference { get; set; }
        public DateTime ChangeTime { get; set; }
    }
}
