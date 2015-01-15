using System;
using TrustworthyCompanion.Tools;
using Windows.Media.Devices;
using Windows.Storage;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkID=390556

namespace TrustworthyCompanion.View.Media {
	/// <summary>
	/// An empty page that can be used on its own or navigated to within a Frame.
	/// </summary>
	public sealed partial class VideoCapturePage : Page {

		private MediaCaptureTool _cameraCapture;
		private StorageFile _videoFile;

		public VideoCapturePage() {
			this.InitializeComponent();
		}

		/// <summary>
		/// Invoked when this page is about to be displayed in a Frame.
		/// </summary>
		/// <param name="e">Event data that describes how this page was reached.
		/// This parameter is typically used to configure the page.</param>
		protected async override void OnNavigatedTo(NavigationEventArgs e) {
			// Init and show preview
			_cameraCapture = new MediaCaptureTool();
			PreviewElement.Source = await _cameraCapture.Initialize(CaptureUse.Video);
			await _cameraCapture.StartPreview();
			_videoFile = null;
		}

		protected override async void OnNavigatedFrom(NavigationEventArgs e) {
			// Release resources
			if(_cameraCapture != null) {
				await _cameraCapture.StopPreview();
				PreviewElement.Source = null;
				_cameraCapture.Dispose();
				_cameraCapture = null;
			}
		}

		private async void BtnRecordVideo_Click(object sender, RoutedEventArgs e) {
			if(_videoFile == null) {
				// Start video recording
				_videoFile = await _cameraCapture.StartVideoRecording();
				BtnRecordVideo.Content = "Stop Video Recording";
			}
			else {
				// Stop video recording
				await _cameraCapture.StopVideoRecording();

				// Start playback
				PlaybackElement.SetSource(await _videoFile.OpenReadAsync(), _videoFile.ContentType);

				// Update UI
				PlaybackElement.Visibility = Visibility.Visible;
				PreviewElement.Visibility = Visibility.Collapsed;
				BtnRecordVideo.Visibility = Visibility.Collapsed;
				BtnRecordVideo.Content = "Start Video Recording";
				_videoFile = null;
			}
		}

		private void PlaybackElement_MediaEnded(object sender, RoutedEventArgs e) {
			PreviewElement.Visibility = Visibility.Visible;
			BtnRecordVideo.Visibility = Visibility.Visible;
			PlaybackElement.Visibility = Visibility.Collapsed;
		}
	}
}
