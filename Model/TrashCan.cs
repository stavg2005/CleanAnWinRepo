﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model
{
    public class TrashCan
    {
        public int id { get; set; }
        public int isfull { get; set; }
        public int weight { get; set; }
        public string longitude { get; set; }
        public string latitude { get; set; }

        public TrashCan(int iD, int isFull, int weight, string longitude,string latitude)
		{
			id = iD;
			isfull = isFull;
			weight = weight;
			this.latitude = latitude;
			this.longitude = longitude;

        }

		public TrashCan()
		{
			id = -1;
			isfull = -1;
			weight = -1;
			latitude = "0";
			longitude = "0";

        }
	}
}
