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

		public string lat {  get; set; }

		public string lng { get; set; }
		public Locations(int iD, string name,string lat,string lng)
		{
			ID = iD;
			Name = name;
			this.lat = lat;
			this.lng = lng;
		}

		public Locations()
		{
			ID =-1;
			Name = null;
			lat = "";
			lng = "";
		}
	}
}
