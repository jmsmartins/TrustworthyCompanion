using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Ioc;
using GalaSoft.MvvmLight.Messaging;
using GalaSoft.MvvmLight.Views;
using TrustworthyCompanion.Database;
using TrustworthyCompanion.Messages;
using TrustworthyCompanion.Model;
using TrustworthyCompanion.Tools;

namespace TrustworthyCompanion.ViewModel.Admin {
	public class AQuestionListViewModel : ViewModelBase {

		// Navigation service
		private INavigationService _navigationService;

		/// <summary>
		/// Initializes a new instance of the AdminBasicInformationViewModel class.
		/// </summary>
		public AQuestionListViewModel(INavigationService navigationService) {
			this._navigationService = navigationService;
			this.ControlLoadedCommand = new RelayCommand(ControlLoaded);
			this.ControlUnloadedCommand = new RelayCommand(ControlUnloaded);
			this.ListItemClick = new RelayCommand<QuestionModel>((item) => ListItemClickHandler(item));
		}

		#region RELAY COMMANDS
		public RelayCommand ControlLoadedCommand { get; private set; }
		public RelayCommand ControlUnloadedCommand { get; private set; }
		public RelayCommand<QuestionModel> ListItemClick { get; private set; }
		#endregion

		#region PROPERTIES
		/// <summary>
		/// The Questions list
		/// </summary>
		private ObservableCollection<QuestionModel> _questionsList;
		public ObservableCollection<QuestionModel> QuestionsList {
			get { return _questionsList; }
			set { Set(() => this.QuestionsList, ref _questionsList, value); }
		}

		/// <summary>
		/// The ListView selection mode property
		/// </summary>
		private bool _multipleSelect;
		public bool MultipleSelect {
			get { return _multipleSelect; }
			set { Set(() => this.MultipleSelect, ref _multipleSelect, value); }
		}

		/// <summary>
		/// The ListViewItem IsClick enabled property
		/// </summary>
		private bool _isClickEnabled;
		public bool IsClickEnabled {
			get { return _isClickEnabled; }
			set { Set(() => this.IsClickEnabled, ref _isClickEnabled, value); }
		}
		#endregion

		/// <summary>
		/// When the control loads, set properties
		/// </summary>
		private async void ControlLoaded() {
			// Subscribe the messenger for messages sent to it
			if(!SimpleIoc.Default.IsRegistered<SelectionModeChange>()) {
				Messenger.Default.Register<SelectionModeChange>(this, (action) => OnSelectionModeChange(action));
			}

			if(!SimpleIoc.Default.IsRegistered<GeneralMessages>()) {
				Messenger.Default.Register<GeneralMessages>(this, (action) => OnMessageReceived(action));
			}

			this.MultipleSelect = false;
			this.IsClickEnabled = true;
			await LoadData();
		}

		/// <summary>
		/// When the control unloads
		/// </summary>
		private void ControlUnloaded() {
			// Unregister the messenger to avoid receiving more messages when it's not open
			Messenger.Default.Unregister<SelectionModeChange>(this, (action) => OnSelectionModeChange(action));
			Messenger.Default.Unregister<GeneralMessages>(this, (action) => OnMessageReceived(action));
		}

		private async Task LoadData() {
			QuestionsList = await DatabaseService.GetQuestions();
		}

		/// <summary>
		/// Handles the click in each ListViewItem
		/// </summary>
		/// <param name="item"></param>
		private void ListItemClickHandler(QuestionModel item) {
			if(!this.MultipleSelect) {
				NavigateTo(new Tuple<string, object>(PagesNames.AQuestionPage, item));
			} else {
				return;
			}
		}

		private void OnSelectionModeChange(SelectionModeChange message) {
			MultipleSelect = message.MultipleSelect;
			IsClickEnabled = !MultipleSelect;
		}

		private async void OnMessageReceived(GeneralMessages message) {
			if(message == GeneralMessages.DELETE_QUESTIONS) {
				List<QuestionModel> toDelete = new List<QuestionModel>();
				foreach(var item in QuestionsList) {
					if(item.IsSelected) {
						toDelete.Add(item);
					}
				}

				// Remove from the list
				foreach(var item in toDelete) {
					QuestionsList.Remove(item);
				}

				// Delete the records from the database
				await DatabaseService.DeleteQuestions(toDelete);
			}
		}

		#region NAVIGATION SERVICE
		public void NavigateTo(Tuple<string, object> args) {
			this._navigationService.NavigateTo(args.Item1, args.Item2);
		}
		#endregion

	}
}
