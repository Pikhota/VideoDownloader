using VDownLoader.Models;

namespace VDownLoader.Services
{
	interface IDownloadVideoService
	{
		void DownloadVideo<T>(string pathTo, T video) where T : Video;
	}
}
