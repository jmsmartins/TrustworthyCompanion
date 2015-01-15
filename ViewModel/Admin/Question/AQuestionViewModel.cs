using System.Threading.Tasks;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Ioc;
using GalaSoft.MvvmLight.Messaging;
using TrustworthyCompanion.Database;
using TrustworthyCompanion.Messages;
using TrustworthyCompanion.Model;

namespace TrustworthyCompanion.ViewModel.Admin.Question {
	public class AQuestionViewModel : ViewModelBase {
		/// <summary>
		/// Initializes a new instance of the AQuestionViewModel class.
		/// </summary>
		public AQuestionViewModel() {
			// Register the messenger
			// Subscribe the messenger for messages sent to it
			if(!SimpleIoc.Default.IsRegistered<QuestionModel>()) {
				Messenger.Default.Register<QuestionModel>(this, (action) => SetupProperties(action));
			}

			this.ControlLoadedCommand = new RelayCommand(ControlLoaded);
		}

		#region RELAY COMMANDS
		public RelayCommand ControlLoadedCommand { get; private set; }
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
		#endregion

		private void ControlLoaded() {
			// Register the messenger
			// Subscribe the messenger for messages sent to it
			if(!SimpleIoc.Default.IsRegistered<GeneralMessages>()) {
				Messenger.Default.Register<GeneralMessages>(this, (action) => SaveQuestion(action));
			}

			//await LoadData();
		}

		private async Task LoadData() {
			//BasicInformation = await DatabaseService.GetQuestion();
		}

		private void SetupProperties(QuestionModel action) {
			Question = action;
		}

		private async void SaveQuestion(GeneralMessages action) {
			if(action == GeneralMessages.SAVE_QUESTION) {
				await DatabaseService.UpdateQuestion(Question);
			}
		}
	}
}
