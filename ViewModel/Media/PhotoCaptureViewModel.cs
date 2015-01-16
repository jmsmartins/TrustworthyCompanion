using System;
using System.IO;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Ioc;
using GalaSoft.MvvmLight.Messaging;
using TrustworthyCompanion.Database;
using TrustworthyCompanion.Model;
using Windows.Storage;

namespace TrustworthyCompanion.ViewModel.Media {
	public class PhotoCaptureViewModel : ViewModelBase {

		//private MediaCaptureTool _mediaCapture;
		//private StorageFile _storageFile = null;

		/// <summary>
		/// Initializes a new instance of the PhotoCaptureViewModel class.
		/// </summary>
		public PhotoCaptureViewModel() {
			// Register the messenger
			// Subscribe the messenger for messages sent to it
			if(!SimpleIoc.Default.IsRegistered<QuestionModel>()) {
				Messenger.Default.Register<QuestionModel>(this, (action) => SetupProperties(action));
			}

			this.PageLoadedCommand = new RelayCommand(PageLoaded);
			this.PageUnloadedCommand = new RelayCommand(PageUnloaded);
			this.CapturePhotoCommand = new RelayCommand<object>((sender) => CapturePhotoHandler(sender));
			this.DeleteCommand = new RelayCommand(DeletePhotoHandler);
		}

		#region RELAY COMMANDS
		public RelayCommand PageLoadedCommand { get; private set; }
		public RelayCommand PageUnloadedCommand { get; private set; }
		public RelayCommand<object> CapturePhotoCommand { get; private set; }
		public RelayCommand DeleteCommand { get; private set; }
		#endregion

		#region PROPERTIES
		/// <summary>
		/// The Question property
		/// </summary>
		private QuestionModel _question;
		public QuestionModel Question {
			get { return _question; }
			set { Set(() => this.Question, ref _question, value); }
		}

		/// <summary>
		/// The Media Source property
		/// </summary>
		/*private MediaCapture _mediaSource;
		public MediaCapture MediaSource {
			get { return _mediaSource; }
			set { Set(() => this.MediaSource, ref _mediaSource, value); }
		}*/

		/// <summary>
		/// The capture enabled property
		/// </summary>
		private bool _captureEnabled;
		public bool CaptureEnabled {
			get { return _captureEnabled; }
			set { Set(() => this.CaptureEnabled, ref _captureEnabled, value); }
		}

		/// <summary>
		/// The photo file property
		/// </summary>
		private string _photoFile;
		public string PhotoFile {
			get { return _photoFile; }
			set {
				Set(() => this.PhotoFile, ref _photoFile, value);
				Question.PhotoFile = PhotoFile;
				DatabaseService.UpdateQuestion(Question);
			}
		}

		/// <summary>
		/// The delete button enabled property
		/// </summary>
		private bool _deleteEnabled;
		public bool DeleteEnabled {
			get { return _deleteEnabled; }
			set { Set(() => this.DeleteEnabled, ref _deleteEnabled, value); }
		}
		#endregion

		private void SetupProperties(QuestionModel action) {
			Question = action;
		}

		/// <summary>
		/// When the page loads, set properties
		/// </summary>
		private void PageLoaded() {
			CaptureEnabled = (Question.PhotoFile != "") ? false : true;
			DeleteEnabled = !CaptureEnabled;
		}

		/// <summary>
		/// When the page unloads
		/// </summary>
		private void PageUnloaded() {
			/*// Release resources
			if(_mediaCapture != null) {
				await _mediaCapture.StopPreview();
				_mediaCapture.Dispose();
				_mediaCapture = null;
			}*/

			// Unregister the messenger to avoid receiving more messages when it's not open
			Messenger.Default.Unregister<QuestionModel>(this, (action) => SetupProperties(action));
		}

		private void CapturePhotoHandler(object sender) {
			// Disable button to prevent exception due to parallel capture usage
			CaptureEnabled = false;
			DeleteEnabled = !CaptureEnabled;
		}

		private async void DeletePhotoHandler() {
			// Then remove the actual file
			var file = await ApplicationData.Current.LocalFolder.GetFileAsync(Path.GetFileName(Question.PhotoFile));
			await file.DeleteAsync(StorageDeleteOption.PermanentDelete);

			// First remove the file from the database
			Question.PhotoFile = "";
			await DatabaseService.UpdateQuestion(Question);

			// Reset the controls
			CaptureEnabled = true;
			DeleteEnabled = !CaptureEnabled;
		}
	}
}