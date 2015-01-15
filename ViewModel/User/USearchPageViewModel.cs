using System;
using System.Linq;
using System.Collections.Generic;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using TrustworthyCompanion.Database;
using TrustworthyCompanion.Model;
using Windows.ApplicationModel.Calls;
using Windows.Media.SpeechRecognition;
using System.Collections.ObjectModel;
using System.Diagnostics;
using Windows.ApplicationModel.Chat;
using Windows.ApplicationModel.Email;
using System.Threading.Tasks;
using TrustworthyCompanion.Tools;
using GalaSoft.MvvmLight.Views;

namespace TrustworthyCompanion.ViewModel.User {
	public class USearchPageViewModel : ViewModelBase {

		private SpeechRecognizer _speechRecognizer;
		private BasicInformationModel _basicInformation;
		private List<QuestionModel> _questionList;
		private List<GeneralQuestionModel> _otherActions;
		// Navigation service
		private INavigationService _navigationService;

		/// <summary>
		/// Initializes a new instance of the AudioCaptureViewModel class.
		/// </summary>
		public USearchPageViewModel(INavigationService navigationService) {
			this._navigationService = navigationService;
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
			if(_speechRecognizer == null) {
				_speechRecognizer = new SpeechRecognizer();
			}

			if(_questionList == null) {
				_questionList = new List<QuestionModel>();
			}

			if(_otherActions == null) {
				_otherActions = new List<GeneralQuestionModel>();
			}

			LoadData();
		}

		private async void LoadData() {
			_basicInformation = await DatabaseService.GetBasicInformation();
			ObservableCollection<QuestionModel> list = await DatabaseService.GetQuestions();
			_questionList = list.ToList();

			
			// General questions
			_otherActions.Add(new GeneralQuestionModel() {
				Id = "call",
				Terms = "call home please help"
			});

			_otherActions.Add(new GeneralQuestionModel() {
				Id = "email",
				Terms = "send email please"
			});

			_otherActions.Add(new GeneralQuestionModel() {
				Id = "sms",
				Terms = "sms send please"
			});
		}

		private async void SearchHandler() {
			// Compile the dictation grammar
			await _speechRecognizer.CompileConstraintsAsync();

			try {
				// Start Recognition
				SpeechRecognitionResult speechRecognitionResult = await _speechRecognizer.RecognizeWithUIAsync();

				if(speechRecognitionResult.Status == SpeechRecognitionResultStatus.Success) {
					HandleSpeechResult(speechRecognitionResult.Text);
				}
			} catch(Exception e) {
				Debug.WriteLine(e.Message);
			}
		}

		private async void HandleSpeechResult(string searchString) {
			// First search the general questions
			foreach(var item in _otherActions) {
				string[] searchTerms = item.Terms.Split(' ');
				if(Search(searchString, searchTerms)) {
					await GeneralQuestionsHandler(item.Id);
					return;
				}
			}

			// Then check the question list
			foreach(var item in _questionList) {
				string[] searchTerms = item.Title.Split(' ');
				if(Search(searchString, searchTerms)) {
					QuestionSearchHandler(item);
					return;
				}
			}
		}


		private async Task GeneralQuestionsHandler(string action) {
			switch(action) {
				case "call":
					PhoneCallManager.ShowPhoneCallUI(_basicInformation.Phone, _basicInformation.Name);
				break;

				case "email":
					EmailMessage mail = new Windows.ApplicationModel.Email.EmailMessage();
					mail.Subject = "Call me";
					mail.Body = "Please call me as soon as possible on this number: " + _basicInformation.Phone;
					mail.To.Add(new EmailRecipient("trustworthycompanion@mailinator.com", "Help contact"));
					await EmailManager.ShowComposeNewEmailAsync(mail);
				break;

				case "sms":
					ChatMessage msg = new Windows.ApplicationModel.Chat.ChatMessage();
					msg.Body = "Please call me as soon as possible.";
					msg.Recipients.Add(_basicInformation.Phone);
					await ChatMessageManager.ShowComposeSmsMessageAsync(msg);
				break;
			}
		}

		private void QuestionSearchHandler(QuestionModel question) {
			NavigateTo(new Tuple<string, object>(PagesNames.UQuestionPage, question));
		}

		public static IEnumerable<string> MatchingStringsCaseInsensitive(string haystack, IEnumerable<string> needles) {
			var h = haystack.ToLower();
			return needles.Where(needle => h.Contains(needle.ToLower()));
		}

		private bool Search(string searchString, string[] searchTerms) {
			var matches = MatchingStringsCaseInsensitive(searchString, searchTerms);
			// Calculate probability
			int wordCount = searchTerms.Count();
			int result = matches.Count();

			double probability = Convert.ToDouble(result) / Convert.ToDouble(wordCount);

			if(probability > 0.75) {
				return true;
			} else {
				return false;
			}
		}

		#region NAVIGATION SERVICE
		public void NavigateTo(Tuple<string, object> args) {
			this._navigationService.NavigateTo(args.Item1, args.Item2);
		}
		#endregion
	
	}
}
