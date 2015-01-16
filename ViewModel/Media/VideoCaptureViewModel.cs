using System;
using System.IO;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Ioc;
using GalaSoft.MvvmLight.Messaging;
using TrustworthyCompanion.Database;
using TrustworthyCompanion.Model;
using Windows.Storage;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;

namespace TrustworthyCompanion.ViewModel.Media {
	public class VideoCaptureViewModel : ViewModelBase {

		private bool _recording = false;

		/// <summary>
		/// Initializes a new instance of the VideoaptureViewModel class.
		/// </summary>
		public VideoCaptureViewModel() {
			// Register the messenger
			// Subscribe the messenger for messages sent to it
			if(!SimpleIoc.Default.IsRegistered<QuestionModel>()) {
				Messenger.Default.Register<QuestionModel>(this, (action) => SetupProperties(action));
			}

			this.PageLoadedCommand = new RelayCommand(PageLoaded);
			this.PageUnloadedCommand = new RelayCommand(PageUnloaded);
			this.CaptureVideoCommand = new RelayCommand<object>((sender) => CaptureVideoHandler(sender));
			this.PlayStopCommand = new RelayCommand<object>((sender) => PlayStopVideoHandler(sender));
			this.MediaEndedCommand = new RelayCommand<object>((item) => MediaEndedHandler(item));
			this.DeleteCommand = new RelayCommand(DeleteVideoHandler);
		}

		#region RELAY COMMANDS
		public RelayCommand PageLoadedCommand { get; private set; }
		public RelayCommand PageUnloadedCommand { get; private set; }
		public RelayCommand<object> CaptureVideoCommand { get; private set; }
		public RelayCommand<object> PlayStopCommand { get; private set; }
		public RelayCommand DeleteCommand { get; private set; }
		public RelayCommand<object> MediaEndedCommand { get; private set; }
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
		/// The capture enabled property
		/// </summary>
		private bool _captureEnabled;
		public bool CaptureEnabled {
			get { return _captureEnabled; }
			set { Set(() => this.CaptureEnabled, ref _captureEnabled, value); }
		}

		/// <summary>
		/// The play/stop button enabled property
		/// </summary>
		private bool _playStopEnabled;
		public bool PlayStopEnabled {
			get { return _playStopEnabled; }
			set { Set(() => this.PlayStopEnabled, ref _playStopEnabled, value); }
		}

		/// <summary>
		/// The Play button checked property
		/// </summary>
		private bool _playButtonChecked;
		public bool PlayButtonChecked {
			get { return _playButtonChecked; }
			set { Set(() => this.PlayButtonChecked, ref _playButtonChecked, value); }
		}

		/// <summary>
		/// The delete button enabled property
		/// </summary>
		private bool _deleteEnabled;
		public bool DeleteEnabled {
			get { return _deleteEnabled; }
			set { Set(() => this.DeleteEnabled, ref _deleteEnabled, value); }
		}

		/// <summary>
		/// The video file property
		/// </summary>
		private string _videoFile;
		public string VideoFile {
			get { return _videoFile; }
			set {
				Set(() => this.VideoFile, ref _videoFile, value);
				Question.VideoFile = VideoFile;
				DatabaseService.UpdateQuestion(Question);
			}
		}
		#endregion

		private void SetupProperties(QuestionModel action) {
			Question = action;
		}

		/// <summary>
		/// When the page loads, set properties
		/// </summary>
		private void PageLoaded() {
			CaptureEnabled = (Question.VideoFile != "") ? false : true;
			PlayStopEnabled = !CaptureEnabled;
			DeleteEnabled = !CaptureEnabled;
			PlayButtonChecked = false;
		}

		/// <summary>
		/// When the page unloads
		/// </summary>
		private void PageUnloaded() {
			// Unregister the messenger to avoid receiving more messages when it's not open
			Messenger.Default.Unregister<QuestionModel>(this, (action) => SetupProperties(action));
		}

		private void CaptureVideoHandler(object sender) {
			// Disable button to prevent exception due to parallel capture usage
			if(!_recording) {
				_recording = true;
				DeleteEnabled = false;
				PlayStopEnabled = false;
			} else {
				CaptureEnabled = false;
				PlayStopEnabled = true;
				DeleteEnabled = true;
				_recording = false;
			}
		}

		private void PlayStopVideoHandler(object sender) {
			MediaElement player = (MediaElement)sender;
			if(player.CurrentState == MediaElementState.Playing) {
				MediaEndedHandler(player);
			} else {
				player.Play();
				DeleteEnabled = PlayStopEnabled;
			}
		}

		private void MediaEndedHandler(object sender) {
			MediaElement element = (MediaElement)sender;
			element.Stop();
			PlayStopEnabled = true;
			DeleteEnabled = PlayStopEnabled;
			PlayButtonChecked = false;
		}

		private async void DeleteVideoHandler() {
			// Then remove the actual file
			var file = await ApplicationData.Current.LocalFolder.GetFileAsync(Path.GetFileName(Question.VideoFile));
			await file.DeleteAsync(StorageDeleteOption.PermanentDelete);

			// First remove the file from the database
			Question.VideoFile = "";
			await DatabaseService.UpdateQuestion(Question);

			// Reset the controls
			CaptureEnabled = true;
			DeleteEnabled = !CaptureEnabled;
			PlayStopEnabled = !CaptureEnabled;
		}
	}
}