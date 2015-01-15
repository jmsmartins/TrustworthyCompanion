using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Ioc;
using GalaSoft.MvvmLight.Messaging;
using GalaSoft.MvvmLight.Views;
using TrustworthyCompanion.Model;
using TrustworthyCompanion.Tools;

namespace TrustworthyCompanion.ViewModel.User.Question {
	public class UQuestionMediaViewModel : ViewModelBase {

		// Navigation service
		private INavigationService _navigationService;
		private QuestionModel _question;

		/// <summary>
		/// Initializes a new instance of the MainViewModel class.
		/// </summary>
		public UQuestionMediaViewModel(INavigationService navigationService) {
			this._navigationService = navigationService;

			// Register the messenger
			// Subscribe the messenger for messages sent to it
			if(!SimpleIoc.Default.IsRegistered<QuestionModel>()) {
				Messenger.Default.Register<QuestionModel>(this, (action) => SetupProperties(action));
			}

			this.ControlUnloadedCommand = new RelayCommand(ControlUnloaded);
			this.MediaShowCommand = new RelayCommand<string>((item) => MediaShowHandler(item));
		}

		#region RELAY COMMANDS
		public RelayCommand ControlUnloadedCommand { get; private set; }
		public RelayCommand<string> MediaShowCommand { get; private set; }
		#endregion

		#region PROPERTIES
		/// <summary>
		/// The Audio Button Visibility property
		/// </summary>
		private bool _audioButtonVisibility;
		public bool AudioButtonVisibility {
			get { return _audioButtonVisibility; }
			set { Set(() => this.AudioButtonVisibility, ref _audioButtonVisibility, value); }
		}

		/// <summary>
		/// The Video Button Visibility property
		/// </summary>
		private bool _videoButtonVisibility;
		public bool VideoButtonVisibility {
			get { return _videoButtonVisibility; }
			set { Set(() => this.VideoButtonVisibility, ref _videoButtonVisibility, value); }
		}

		/// <summary>
		/// The Photo Button Visibility property
		/// </summary>
		private bool _photoButtonVisibility;
		public bool PhotoButtonVisibility {
			get { return _photoButtonVisibility; }
			set { Set(() => this.PhotoButtonVisibility, ref _photoButtonVisibility, value); }
		}
		#endregion

		private void ControlUnloaded() {
			Messenger.Default.Unregister<QuestionModel>(this, (action) => SetupProperties(action));
		}

		private void SetupProperties(QuestionModel action) {
			_question = action;

			AudioButtonVisibility = (_question.AudioFile != "") ? true : false;
			PhotoButtonVisibility = (_question.PhotoFile != "") ? true : false;
			VideoButtonVisibility = (_question.VideoFile != "") ? true : false;
		}

		private void MediaShowHandler(string button) {
			switch(button) {
				case "Audio":
					this._navigationService.NavigateTo(PagesNames.AudioShowPage, _question);
				break;

				case "Photo":
					this._navigationService.NavigateTo(PagesNames.PhotoShowPage, _question);
				break;

				case "Video":
					this._navigationService.NavigateTo(PagesNames.VideoShowPage, _question);
				break;
			}
		}
	}
}
