using GalaSoft.MvvmLight.Messaging;
using TrustworthyCompanion.Model;
using TrustworthyCompanion.Tools;
using Windows.Storage;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media.Imaging;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkID=390556

namespace TrustworthyCompanion.View.Media {
	/// <summary>
	/// An empty page that can be used on its own or navigated to within a Frame.
	/// </summary>
	public sealed partial class PhotoCapturePage : Page {

		private MediaCaptureTool _mediaCapture;
		private StorageFile _storageFile = null;

		public PhotoCapturePage() {
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
			CameraCapture.Source = await _mediaCapture.Initialize();
			await _mediaCapture.StartPreview();
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

		private async void Button_Click(object sender, Windows.UI.Xaml.RoutedEventArgs e) {
			_storageFile = await _mediaCapture.CapturePhoto();
			CapturedPhoto.Tag = _storageFile.Path;
		}
	}
}
