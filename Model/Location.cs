using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model
{
	public class Locations
	{
		public int ID { get; set; }
		public string Name { get; set; }

		public Locations(int iD, string name)
		{
			ID = iD;
			Name = name;
		}

		public Locations()
		{
			ID =-1;
			Name = null;
		}
	}
}
