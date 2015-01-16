using System;
using System.IO;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Ioc;
using GalaSoft.MvvmLight.Messaging;
using TrustworthyCompanion.Database;
using TrustworthyCompanion.Model;
using TrustworthyCompanion.Tools;
using Windows.Storage;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;

namespace TrustworthyCompanion.ViewModel.Media {
	public class AudioCaptureViewModel : ViewModelBase {

		private MediaCaptureTool _mediaCapture;
		private StorageFile _storageFile = null;
		private bool _recording = false;

		/// <summary>
		/// Initializes a new instance of the AudioCaptureViewModel class.
		/// </summary>
		public AudioCaptureViewModel() {
			// Register the messenger
			// Subscribe the messenger for messages sent to it
			if(!SimpleIoc.Default.IsRegistered<QuestionModel>()) {
				Messenger.Default.Register<QuestionModel>(this, (action) => SetupProperties(action));
			}

			this.PageLoadedCommand = new RelayCommand(PageLoaded);
			this.PageUnloadedCommand = new RelayCommand(PageUnloaded);
			this.RecordAudioCommand = new RelayCommand(RecordAudioHandler);
			this.DeleteCommand = new RelayCommand(DeleteAudioHandler);
			this.PlayStopCommand = new RelayCommand<object>((item) => PlayStopAudioHandler(item));
			this.MediaEndedCommand = new RelayCommand<object>((item) => MediaEndedHandler(item));
		}

		#region RELAY COMMANDS
		public RelayCommand PageLoadedCommand { get; private set; }
		public RelayCommand PageUnloadedCommand { get; private set; }
		public RelayCommand RecordAudioCommand { get; private set; }
		public RelayCommand DeleteCommand { get; private set; }
		public RelayCommand<object> PlayStopCommand { get; private set; }
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
		/// The play/stop enabled property
		/// </summary>
		private bool _playStopEnabled;
		public bool PlayStopEnabled{
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
		/// The record enabled property
		/// </summary>
		private bool _recordEnabled;
		public bool RecordEnabled {
			get { return _recordEnabled; }
			set { Set(() => this.RecordEnabled, ref _recordEnabled, value); }
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
		private async void PageLoaded() {
			PlayStopEnabled = (Question.AudioFile != "") ? true : false;
			RecordEnabled = (Question.AudioFile != "") ? false : true;
			DeleteEnabled = !RecordEnabled;
			PlayButtonChecked = false;

			// Init
			_mediaCapture = new MediaCaptureTool();
			await _mediaCapture.Initialize();
		}

		/// <summary>
		/// When the page unloads
		/// </summary>
		private void PageUnloaded() {
			// Release resources
			if(_mediaCapture != null) {
				_mediaCapture.Dispose();
				_mediaCapture = null;
			}

			// Unregister the messenger to avoid receiving more messages when it's not open
			Messenger.Default.Unregister<QuestionModel>(this, (action) => SetupProperties(action));
		}

		private async void RecordAudioHandler() {
			if(!_recording) {
				_recording = true;
				PlayStopEnabled = false;
				RecordEnabled = true;
				DeleteEnabled = false;
				// Start video recording
				_storageFile = await _mediaCapture.StartAudioRecording();
			} else {
				_recording = false;
				PlayStopEnabled = true;
				RecordEnabled = false;
				DeleteEnabled = true;
				// Stop video recording
				await _mediaCapture.StopAudioRecording();

				// Save the info to the database
				Question.AudioFile = _storageFile.Path;
				await DatabaseService.UpdateQuestion(Question);
			}
		}

		private void PlayStopAudioHandler(object sender) {
			MediaElement element = (MediaElement)sender;
			if(element.CurrentState == MediaElementState.Playing) {
				MediaEndedHandler(element);
			} else {
				element.Play();
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

		private async void DeleteAudioHandler() {
			// Then remove the actual file
			var file = await ApplicationData.Current.LocalFolder.GetFileAsync(Path.GetFileName(Question.AudioFile));
			await file.DeleteAsync(StorageDeleteOption.PermanentDelete);

			// First remove the file from the database
			Question.AudioFile = "";
			await DatabaseService.UpdateQuestion(Question);

			// Reset the controls
			RecordEnabled = true;
			DeleteEnabled = !RecordEnabled;
			PlayStopEnabled = !RecordEnabled;
		}
	}
}
