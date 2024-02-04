using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model
{
    public class TrashCan
    {
        public int ID { get; set; }
        public int IsFull { get; set; }
        public int Weight { get; set; }
        public string longitude { get; set; }
        public string latitude { get; set; }

        public TrashCan(int iD, int isFull, int weight, string longitude,string latitude)
		{
			ID = iD;
			IsFull = isFull;
			Weight = weight;
			this.latitude = latitude;
			this.longitude = longitude;

        }

		public TrashCan()
		{
			ID = -1;
			IsFull = -1;
			Weight = -1;
			latitude = "0";
			longitude = "0";

        }
	}
}
