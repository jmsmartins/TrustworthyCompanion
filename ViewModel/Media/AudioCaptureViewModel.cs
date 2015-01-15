using System;
using System.Diagnostics;
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

namespace TrustworthyCompanion.ViewModel.Media {
	public class AudioCaptureViewModel : ViewModelBase {

		private MediaCaptureTool _mediaCapture;
		private StorageFile _audioStorageFile = null;
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
			this.DeleteAudioCommand = new RelayCommand(DeleteAudioHandler);
			this.PlayAudioCommand = new RelayCommand<object>((item) => PlayAudioHandler(item));
		}

		#region RELAY COMMANDS
		public RelayCommand PageLoadedCommand { get; private set; }
		public RelayCommand PageUnloadedCommand { get; private set; }
		public RelayCommand RecordAudioCommand { get; private set; }
		public RelayCommand SaveAudioCommand { get; private set; }
		public RelayCommand DeleteAudioCommand { get; private set; }
		public RelayCommand<object> PlayAudioCommand { get; private set; }
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
		/// The play enabled property
		/// </summary>
		private bool _playEnabled;
		public bool PlayEnabled{
			get { return _playEnabled; }
			set { Set(() => this.PlayEnabled, ref _playEnabled, value); }
		}

		/// <summary>
		/// The record enabled property
		/// </summary>
		private bool _recordEnabled;
		public bool RecordEnabled {
			get { return _recordEnabled; }
			set { Set(() => this.RecordEnabled, ref _recordEnabled, value); }
		}
		#endregion

		private void SetupProperties(QuestionModel action) {
			Question = action;
		}

		/// <summary>
		/// When the page loads, set properties
		/// </summary>
		private async void PageLoaded() {
			PlayEnabled = (Question.AudioFile != "") ? true : false;
			RecordEnabled = (Question.AudioFile != "") ? false : true;

			// Init
			_mediaCapture = new MediaCaptureTool();
			await _mediaCapture.Initialize();
		}

		/// <summary>
		/// When the page unloads
		/// </summary>
		private async void PageUnloaded() {
			// Release resources
			if(_mediaCapture != null) {
				_mediaCapture.Dispose();
				_mediaCapture = null;
			}
		}

		
		private async void RecordAudioHandler() {

			if(!_recording) {
				_recording = true;
				PlayEnabled = false;
				RecordEnabled = true;
				// Start video recording
				_audioStorageFile = await _mediaCapture.StartAudioRecording();
			} else {
				_recording = false;
				PlayEnabled = true;
				RecordEnabled = false;
				// Stop video recording
				await _mediaCapture.StopAudioRecording();

				// Save the info to the database
				Question.AudioFile = _audioStorageFile.Path;
				await DatabaseService.UpdateQuestion(Question);
			}
		}

		private void PlayAudioHandler(object sender) {
			MediaElement element = (MediaElement)sender;
			element.Play();
		}

		private async void DeleteAudioHandler() {
			// Then remove the actual file
			var file = await ApplicationData.Current.LocalFolder.GetFileAsync(Path.GetFileName(Question.AudioFile));
			await file.DeleteAsync(StorageDeleteOption.PermanentDelete);

			// First remove the file from the database
			Question.AudioFile = "";
			await DatabaseService.UpdateQuestion(Question);

			// Reset the controls
			PlayEnabled = false;
			RecordEnabled = true;
		}
	}
}
