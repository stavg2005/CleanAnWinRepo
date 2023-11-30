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
		public string Coordinates;

		public TrashCan(int iD, int location, int isFull, int weight, string coordinates)
		{
			ID = iD;
			Location = location;
			IsFull = isFull;
			Weight = weight;
			Coordinates = coordinates;
		}

		public TrashCan()
		{
			ID = -1;
			Location = -1;
			IsFull = -1;
			Weight = -1;
			Coordinates = "";
		}
	}
}
