using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Ioc;
using GalaSoft.MvvmLight.Messaging;
using TrustworthyCompanion.Database;
using TrustworthyCompanion.Model;
using Windows.Storage;
using Windows.UI.Xaml.Controls;

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
			this.PlayCommand = new RelayCommand<object>((sender) => PlayVideoHandler(sender));
			this.StopCommand = new RelayCommand<object>((sender) => StopVideoHandler(sender));
			this.DeleteCommand = new RelayCommand(DeleteVideoHandler);
		}

		#region RELAY COMMANDS
		public RelayCommand PageLoadedCommand { get; private set; }
		public RelayCommand PageUnloadedCommand { get; private set; }
		public RelayCommand<object> CaptureVideoCommand { get; private set; }
		public RelayCommand<object> PlayCommand { get; private set; }
		public RelayCommand<object> StopCommand { get; private set; }
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
		/// The capture enabled property
		/// </summary>
		private bool _captureEnabled;
		public bool CaptureEnabled {
			get { return _captureEnabled; }
			set { Set(() => this.CaptureEnabled, ref _captureEnabled, value); }
		}

		/// <summary>
		/// The play button enabled property
		/// </summary>
		private bool _playEnabled;
		public bool PlayEnabled {
			get { return _playEnabled; }
			set { Set(() => this.PlayEnabled, ref _playEnabled, value); }
		}

		/// <summary>
		/// The stop button enabled property
		/// </summary>
		private bool _stopEnabled;
		public bool StopEnabled {
			get { return _stopEnabled; }
			set { Set(() => this.StopEnabled, ref _stopEnabled, value); }
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
			PlayEnabled = !CaptureEnabled;
			StopEnabled = false;
			DeleteEnabled = !CaptureEnabled;
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
				PlayEnabled = false;
				StopEnabled = PlayEnabled;
			} else {
				CaptureEnabled = false;
				PlayEnabled = true;
				StopEnabled = !PlayEnabled;
				DeleteEnabled = true;
				_recording = false;
			}
		}

		private void PlayVideoHandler(object sender) {
			MediaElement player = (MediaElement)sender;
			player.Play();
			PlayEnabled = false;
			DeleteEnabled = PlayEnabled;
			StopEnabled = !PlayEnabled;
		}

		private void StopVideoHandler(object sender) {
			MediaElement player = (MediaElement)sender;
			player.Stop();
			PlayEnabled = true;
			DeleteEnabled = PlayEnabled;
			StopEnabled = !PlayEnabled;
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
			PlayEnabled = !CaptureEnabled;
			StopEnabled = PlayEnabled;
		}
	}
}