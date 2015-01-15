using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using TrustworthyCompanion.Tools;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Imaging;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkID=390556

namespace TrustworthyCompanion.View.Media {
	/// <summary>
	/// An empty page that can be used on its own or navigated to within a Frame.
	/// </summary>
	public sealed partial class PhotoCapturePage : Page {

		private MediaCaptureTool _cameraCapture;

		public PhotoCapturePage() {
			this.InitializeComponent();
		}

		/// <summary>
		/// Invoked when this page is about to be displayed in a Frame.
		/// </summary>
		/// <param name="e">Event data that describes how this page was reached.
		/// This parameter is typically used to configure the page.</param>
		//protected override void OnNavigatedTo(NavigationEventArgs e) {
		//}

		protected override async void OnNavigatedTo(NavigationEventArgs e) {
			// Init and show preview
			_cameraCapture = new MediaCaptureTool();
			PreviewElement.Source = await _cameraCapture.Initialize();
			await _cameraCapture.StartPreview();
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

		private async void BtnCapturePhoto_Click(object sender, RoutedEventArgs e) {
			// Take snapshot and add to ListView
			// Disable button to prevent exception due to parallel capture usage
			BtnCapturePhoto.IsEnabled = false;
			var photoStorageFile = await _cameraCapture.CapturePhoto();

			var bitmap = new BitmapImage();
			await bitmap.SetSourceAsync(await photoStorageFile.OpenReadAsync());
			PhotoListView.Items.Add(bitmap);
			BtnCapturePhoto.IsEnabled = true;
		}
	}
}
