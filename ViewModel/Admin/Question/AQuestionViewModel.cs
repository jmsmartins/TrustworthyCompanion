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
			// Subscribe the messenger for messages sent to it
			if(!SimpleIoc.Default.IsRegistered<QuestionModel>()) {
				Messenger.Default.Register<QuestionModel>(this, (action) => SetupProperties(action));
			}

			this.ControlLoadedCommand = new RelayCommand(ControlLoaded);
			this.ControlUnloadedCommand = new RelayCommand(ControlUnloaded);
		}

		#region RELAY COMMANDS
		public RelayCommand ControlLoadedCommand { get; private set; }
		public RelayCommand ControlUnloadedCommand { get; private set; }
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

		private void SetupProperties(QuestionModel action) {
			Question = action;
		}

		/// <summary>
		/// When the control loads, set properties
		/// </summary>
		private void ControlLoaded() {
			//
		}

		/// <summary>
		/// When the control unloads
		/// </summary>
		private void ControlUnloaded() {
			// Unregister the messenger to avoid receiving more messages when it's not open
			Messenger.Default.Unregister<QuestionModel>(this, (action) => SetupProperties(action));
		}		

		private async void SaveQuestion(GeneralMessages action) {
			if(action == GeneralMessages.SAVE_QUESTION) {
				await DatabaseService.UpdateQuestion(Question);
			}
		}
	}
}
