using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Ioc;
using GalaSoft.MvvmLight.Messaging;
using GalaSoft.MvvmLight.Views;
using TrustworthyCompanion.Messages;
using TrustworthyCompanion.Model;
using TrustworthyCompanion.Tools;
using Windows.Media.Capture;
using Windows.UI.Xaml.Controls;

namespace TrustworthyCompanion.ViewModel.Admin.Question {
	public class AQuestionMediaViewModel : ViewModelBase {

		// Navigation service
		private INavigationService _navigationService;
		private QuestionModel _question;

		/// <summary>
		/// Initializes a new instance of the MainViewModel class.
		/// </summary>
		public AQuestionMediaViewModel(INavigationService navigationService) {
			this._navigationService = navigationService;

			// Register the messenger
			// Subscribe the messenger for messages sent to it
			if(!SimpleIoc.Default.IsRegistered<QuestionModel>()) {
				Messenger.Default.Register<QuestionModel>(this, (action) => SetupProperties(action));
			}

			this.ControlLoadedCommand = new RelayCommand(ControlLoaded);
			this.MediaAddCommand = new RelayCommand<string>((item) => MediaAddHandler(item));
		}

		#region RELAY COMMANDS
		public RelayCommand ControlLoadedCommand { get; private set; }
		public RelayCommand<string> MediaAddCommand { get; private set; }
		#endregion

		#region PROPERTIES
		#endregion

		private void ControlLoaded() {

		}

		private void SetupProperties(QuestionModel action) {
			_question = action;
		}

		private void MediaAddHandler(string button) {
			switch(button) {
				case "Audio":
					this._navigationService.NavigateTo(PagesNames.AudioCapturePage, _question);
				break;

				case "Photo":
					this._navigationService.NavigateTo(PagesNames.PhotoCapturePage, _question);
				break;

				case "Video":
					this._navigationService.NavigateTo(PagesNames.VideoCapturePage, "");
				break;
			}
		}
	}
}
