using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model
{
	public class ImageModel
	{
		public byte[] ImageData { get; set; }
		public string contentType { get; set; }
		public string FileName { get; set; }
	}
}
