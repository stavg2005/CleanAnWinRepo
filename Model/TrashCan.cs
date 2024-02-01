using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model
{
    public class TrashCan
    {
        public int ID;
        public int Location;
        public int IsFull;
		public int Weight;
		public string longitude;
		public string latitude;

		public TrashCan(int iD, int location, int isFull, int weight, string longitude,string latitude)
		{
			ID = iD;
			Location = location;
			IsFull = isFull;
			Weight = weight;
			this.latitude = latitude;
			this.longitude = longitude;

        }

		public TrashCan()
		{
			ID = 43;
			Location = 1;
			IsFull = -1;
			Weight = -1;
			latitude = "0";
			longitude = "0";

        }
	}
}
