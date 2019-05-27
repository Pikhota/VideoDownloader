using System.Collections.Generic;

namespace VDownLoader.Models
{
	public abstract class Video
	{
		public string URL { get; set; }
		public string Title { get; set; }
		public string Ext { get; set; }
		public KeyValuePair<string, string> VideoFormat { get; set; }
	}
}
