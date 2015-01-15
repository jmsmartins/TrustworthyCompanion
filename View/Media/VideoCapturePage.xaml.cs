using GalaSoft.MvvmLight.Messaging;
using TrustworthyCompanion.Model;
using TrustworthyCompanion.Tools;
using Windows.Media.Devices;
using Windows.Storage;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkID=390556

namespace TrustworthyCompanion.View.Media {
	/// <summary>
	/// An empty page that can be used on its own or navigated to within a Frame.
	/// </summary>
	public sealed partial class VideoCapturePage : Page {

		private MediaCaptureTool _mediaCapture;
		private StorageFile _storageFile = null;
		private bool _recording = false;

		public VideoCapturePage() {
			this.InitializeComponent();
		}

		/// <summary>
		/// Invoked when this page is about to be displayed in a Frame.
		/// </summary>
		/// <param name="e">Event data that describes how this page was reached.
		/// This parameter is typically used to configure the page.</param>
		protected async override void OnNavigatedTo(NavigationEventArgs e) {
			QuestionModel message = (QuestionModel)e.Parameter;
			Messenger.Default.Send<QuestionModel>(message);

			// Init and show preview
			_mediaCapture = new MediaCaptureTool();
			CameraCapture.Source = await _mediaCapture.Initialize(CaptureUse.Video);
			await _mediaCapture.StartPreview();
			_storageFile = null;
		}

		protected override async void OnNavigatedFrom(NavigationEventArgs e) {
			// Release resources
			if(_mediaCapture != null) {
				await _mediaCapture.StopPreview();
				CameraCapture.Source = null;
				_mediaCapture.Dispose();
				_mediaCapture = null;
			}
		}

		/*private async void BtnRecordVideo_Click(object sender, RoutedEventArgs e) {
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
		}*/

		private async void CaptureVideoButton_Click(object sender, RoutedEventArgs e) {
			if(!_recording) {
				_recording = true;
				_storageFile = await _mediaCapture.StartVideoRecording();
			} else {
				// Stop video recording
				await _mediaCapture.StopVideoRecording();
				CapturedVideo.Tag = _storageFile.Path;
				_recording = false;

				// Start playback
				//PlaybackElement.SetSource(await _videoFile.OpenReadAsync(), _videoFile.ContentType);
			}
		}
	}
}
