using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.IO;
using System.Net;
using System.Web;
using VDownLoader.Models;

namespace VDownLoader.Services
{
	public abstract class DownloadVideoService : IDownloadVideoService
	{
		protected Uri Uri { get; set; }
		public Video Video { get; set; }
		public Dictionary<string,string> Formats { get; set; }

		public DownloadVideoService(string url)
		{
			Video = new YoutubeVideo();
			if (!string.IsNullOrWhiteSpace(url))
			{
				Uri = new Uri(url);

				Formats = new Dictionary<string, string>();
				Video = InitialInfoVideo(Uri);
			}
		}

		public void DownloadVideo<T>(string pathTo, T video) where T : Video
		{
			NameValueCollection urlParams = HttpUtility.ParseQueryString(video.VideoFormat.Value);

			string filePath = Path.Combine(pathTo, $"{video.Title}.{video.Ext}");

			WebClient webClient = new WebClient();
			webClient.DownloadFileCompleted += new AsyncCompletedEventHandler(Completed);
			webClient.DownloadFile(new Uri(video.VideoFormat.Value), filePath);
		}

		private void Completed(object sender, AsyncCompletedEventArgs e)
		{
			throw new NotImplementedException();
		}

		protected abstract Video InitialInfoVideo(Uri uri);
		
	}
}
