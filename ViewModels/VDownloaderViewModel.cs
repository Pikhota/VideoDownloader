using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Windows.Forms;
using VDownLoader.Commands;
using VDownLoader.Models;
using VDownLoader.Services;

namespace VDownLoader.ViewModels
{
	public class VDownloaderViewModel : INotifyPropertyChanged
	{
		#region private members
		private DownloadYoutubeVideoService _videoService;
		private Video _video;
		private string _url;
		private string _downloadPath;
		private string _selectedFormat;
		private IList<string> _formats;
		private const string youtube = "www.youtube.com";
		private const string kinogo = "kinogo.by";
		private const string topbuzz = "www.topbuzz.com";
		#endregion

		public VDownloaderViewModel()
		{
			_formats = new List<string>();
		}

		#region public properties
		public string DownloadPath
		{
			get { return _downloadPath; }

			set
			{
				_downloadPath = value;
				OnPropertyChanged();
			}
		}

		public string Url
		{
			get { return _url; }

			set
			{
				_url = value;
				if(_url != null)
				{
					Uri uri = new Uri(_url);

					switch (uri.Host)
					{
						case youtube:
							_video = new YoutubeVideo();
							break;
						case topbuzz:
							_video = new TopBuzzVideo();
							break;
						case kinogo:
							_video = new KinogoVideo();
							break;
					}

					_videoService = new DownloadYoutubeVideoService(_url);

					_video = _videoService.Video;

					foreach (var format in _videoService.Formats.Keys)
					{
						Formats.Add(format);
					}
				}

				OnPropertyChanged();
			}
		}

		public string SelectedFormat
		{
			get { return _selectedFormat; }

			set
			{
				_selectedFormat = value;

				foreach(var format in _videoService.Formats)
				{
					if (format.Key == _selectedFormat)
					{
						_video.VideoFormat = format;
					}
				}

				OnPropertyChanged();
			}
		}

		public IList<string> Formats
		{
			get { return _formats; }

			set
			{
				_formats = value;
				OnPropertyChanged();
			}
		}
		#endregion

		#region commands
		public DelegateCommand SetDownloadPath
		{
			get
			{
				return new DelegateCommand((obj) =>
				{
					using (FolderBrowserDialog browser = new FolderBrowserDialog())
					{
						browser.ShowDialog();
						DownloadPath = browser.SelectedPath;
					}
				});
			}
		}

		public DelegateCommand OpenExplorer
		{
			get
			{
				return new DelegateCommand((obj) =>
				{
					string path = obj as string;
					if (!string.IsNullOrWhiteSpace(path))
					{
						Process process = new Process();
						process.StartInfo.UseShellExecute = true;
						process.StartInfo.FileName = path;
						process.Start();
					}
				});
			}
		}

		public DelegateCommand DownloadVideo
		{
			get
			{
				return new DelegateCommand((obj) =>
				{
					_videoService.DownloadVideo(DownloadPath, _video);
				});
			}
		}
		#endregion

		#region INotifyPropertyChange realization
		public event PropertyChangedEventHandler PropertyChanged;
		public void OnPropertyChanged([CallerMemberName]string prop = "")
		{
			PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(prop));
		}
		#endregion
	}
}
