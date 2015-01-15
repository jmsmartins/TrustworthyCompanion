using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Windows.Media;
using Windows.Media.Capture;
using Windows.Media.Devices;
using Windows.Media.MediaProperties;
using Windows.Storage;

namespace TrustworthyCompanion.Tools {
	public class MediaCaptureTool : IDisposable {
		private MediaCapture _mediaCapture;
		private ImageEncodingProperties _imgEncodingProperties;
		private MediaEncodingProfile _videoEncodingProperties;
		private MediaEncodingProfile _audioEncodingProperties;

		public VideoDeviceController VideoDeviceController {
			get { return _mediaCapture.VideoDeviceController; }
		}

		public async Task<MediaCapture> Initialize(CaptureUse primaryUse = CaptureUse.Photo) {
			// Create _mediaCapture and init
			_mediaCapture = new MediaCapture();
			await _mediaCapture.InitializeAsync();
			_mediaCapture.VideoDeviceController.PrimaryUse = primaryUse;

			// Create photo encoding properties as JPEG and set the size that should be used for photo capturing
			_imgEncodingProperties = ImageEncodingProperties.CreateJpeg();
			_imgEncodingProperties.Width = 640;
			_imgEncodingProperties.Height = 480;

			// Create video encoding profile as MP4 
			_videoEncodingProperties = MediaEncodingProfile.CreateMp4(VideoEncodingQuality.Vga);
			// Lots of properties for audio and video could be set here...

			// Create audio encoding profile as MP3
			_audioEncodingProperties = MediaEncodingProfile.CreateM4a(AudioEncodingQuality.Auto);

			return _mediaCapture;
		}

		public async Task<StorageFile> CapturePhoto(string desiredName = "photo.jpg") {
			// Create new unique file in the pictures library and capture photo into it
			//var photoStorageFile = await KnownFolders.PicturesLibrary.CreateFileAsync(desiredName, CreationCollisionOption.GenerateUniqueName);
			var photoStorageFile = await ApplicationData.Current.LocalFolder.CreateFileAsync(desiredName, CreationCollisionOption.GenerateUniqueName);
			await _mediaCapture.CapturePhotoToStorageFileAsync(_imgEncodingProperties, photoStorageFile);
			return photoStorageFile;
		}

		public async Task<StorageFile> StartVideoRecording(string desiredName = "video.mp4") {
			// Create new unique file in the videos library and record video! 
			//var videoStorageFile = await KnownFolders.VideosLibrary.CreateFileAsync(desiredName, CreationCollisionOption.GenerateUniqueName);
			var videoStorageFile = await ApplicationData.Current.LocalFolder.CreateFileAsync(desiredName, CreationCollisionOption.GenerateUniqueName);
			await _mediaCapture.StartRecordToStorageFileAsync(_videoEncodingProperties, videoStorageFile);
			return videoStorageFile;
		}

		public async Task StopVideoRecording() {
			// Stop video recording
			await _mediaCapture.StopRecordAsync();
		}

		public async Task StartPreview() {
			// Start Preview stream
			await _mediaCapture.StartPreviewAsync();
		}
		public async Task StartPreview(IMediaExtension previewSink, double desiredPreviewArea) {
			// List of supported video preview formats to be used by the default preview format selector.
			var supportedVideoFormats = new List<string> { "nv12", "rgb32" };

			// Find the supported preview size that's closest to the desired size
			var availableMediaStreamProperties =
				_mediaCapture.VideoDeviceController.GetAvailableMediaStreamProperties(MediaStreamType.VideoPreview)
					.OfType<VideoEncodingProperties>()
					.Where(p => p != null && !String.IsNullOrEmpty(p.Subtype) && supportedVideoFormats.Contains(p.Subtype.ToLower()))
					.OrderBy(p => Math.Abs(p.Height * p.Width - desiredPreviewArea))
					.ToList();
			var previewFormat = availableMediaStreamProperties.FirstOrDefault();

			// Start Preview stream
			await _mediaCapture.VideoDeviceController.SetMediaStreamPropertiesAsync(MediaStreamType.VideoPreview, previewFormat);
			await _mediaCapture.StartPreviewToCustomSinkAsync(new MediaEncodingProfile { Video = previewFormat }, previewSink);
		}

		public async Task StopPreview() {

			// Stop Preview stream
			await _mediaCapture.StopPreviewAsync();
		}

		public async Task<StorageFile> StartAudioRecording(string desiredName = "audio.m4a") {
			// Create new unique file in the pictures library and capture photo into it
			//var photoStorageFile = await KnownFolders.PicturesLibrary.CreateFileAsync(desiredName, CreationCollisionOption.GenerateUniqueName);
			var audioStorageFile = await ApplicationData.Current.LocalFolder.CreateFileAsync(desiredName, CreationCollisionOption.GenerateUniqueName);
			await _mediaCapture.StartRecordToStorageFileAsync(_audioEncodingProperties, audioStorageFile);
			return audioStorageFile;
		}

		public async Task StopAudioRecording() {
			await _mediaCapture.StopRecordAsync();
		}


		public void Dispose() {
			if(_mediaCapture != null) {
				_mediaCapture.Dispose();
				_mediaCapture = null;
			}
		}
	}
}