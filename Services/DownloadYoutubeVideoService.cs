using System;
using System.Collections.Specialized;
using System.ComponentModel;
using System.IO;
using System.Net;
using System.Text;
using System.Web;
using VDownLoader.Models;

namespace VDownLoader.Services
{
	public  class DownloadYoutubeVideoService : DownloadVideoService
	{
		public DownloadYoutubeVideoService(string url) : base(url) {}

		public void Completed(object sender, AsyncCompletedEventArgs e)
		{
		}

		protected override Video InitialInfoVideo(Uri uri)
		{
			string videoId = HttpUtility.ParseQueryString(uri.Query).Get("v");
			string videoInfoUrl = $"https://www.youtube.com/get_video_info?video_id={videoId}";

			HttpWebRequest request = WebRequest.CreateHttp(videoInfoUrl);
			HttpWebResponse response = (HttpWebResponse)request.GetResponse();
			using (Stream stream = response.GetResponseStream())
			{
				using (StreamReader reader = new StreamReader(stream, Encoding.GetEncoding("utf-8")))
				{
					string videoInfo = HttpUtility.HtmlDecode(reader.ReadToEnd());

					NameValueCollection videoParams = HttpUtility.ParseQueryString(videoInfo);

					if (videoParams["reason"] != null)
					{
						string msg = videoParams["reason"];
						throw new Exception(msg);
					}

					string[] videoURLs = videoParams["url_encoded_fmt_stream_map"].Split(',');

					Video video = new YoutubeVideo();

					foreach (string videoURL in videoURLs)
					{
						string url = HttpUtility.HtmlDecode(videoURL);

						NameValueCollection urlParams = HttpUtility.ParseQueryString(url);
						string videoUrl = HttpUtility.HtmlDecode(urlParams["url"]);
						string videoFormat = HttpUtility.HtmlDecode(urlParams["type"]);
						string videoSignature = HttpUtility.HtmlDecode(urlParams["sig"]);
						video.Title = HttpUtility.HtmlDecode(videoParams["title"]);
						video.Ext = videoFormat.Split(';')[0].Split('/')[1];

						url = $"{videoUrl}&signature={videoSignature}&type={videoFormat}&title={video.Title}";

						videoFormat = $"{urlParams["quality"]} - {video.Ext}";

						Formats.Add(videoFormat, url);
					}

					return video;
				}
			}
		}
	}
}
