using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using Windows.Media.SpeechRecognition;

namespace TrustworthyCompanion.ViewModel.User {
	public class ULandingPageViewModel : ViewModelBase {

		private SpeechRecognizer _speechRecog;

		/// <summary>
		/// Initializes a new instance of the AudioCaptureViewModel class.
		/// </summary>
		public ULandingPageViewModel() {
			this.PageLoadedCommand = new RelayCommand(PageLoaded);
			this.SearchCommand = new RelayCommand(SearchHandler);
		}

		#region RELAY COMMANDS
		public RelayCommand PageLoadedCommand { get; private set; }
		public RelayCommand SearchCommand { get; private set; }
		#endregion

		#region PROPERTIES
		/// <summary>
		/// The Question property
		/// </summary>
		/*private QuestionModel _question;
		public QuestionModel Question {
			get { return _question; }
			set { Set(() => this.Question, ref _question, value); }
		}*/
		#endregion

		/// <summary>
		/// When the page loads, set properties
		/// </summary>
		private void PageLoaded() {
			if(_speechRecog == null) {
				_speechRecog = new SpeechRecognizer();
			}
		}

		private async void SearchHandler() {
			// Compile the dictation grammar
			await _speechRecog.CompileConstraintsAsync();

			// Start Recognition
			SpeechRecognitionResult speechRecognitionResult = await _speechRecog.RecognizeWithUIAsync();
		}
	}
}
