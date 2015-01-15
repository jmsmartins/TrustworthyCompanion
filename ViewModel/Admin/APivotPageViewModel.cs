using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Messaging;
using GalaSoft.MvvmLight.Views;
using TrustworthyCompanion.Messages;
using TrustworthyCompanion.Tools;
using Windows.UI.Xaml.Controls;

namespace TrustworthyCompanion.ViewModel.Admin {
	public class APivotPageViewModel : ViewModelBase {

		private INavigationService _navigationService;

		/// <summary>
		/// Initializes a new instance of the AdminPivotPageViewModel class.
		/// </summary>
		public APivotPageViewModel(INavigationService navigationService) {
			this._navigationService = navigationService;

			InitViewModel();
        }

		private void InitViewModel() {
			SelectedIndex = 0;

			this.NewQuestionCommand = new RelayCommand(NewQuestionHandler);
			this.SaveCommand = new RelayCommand(SaveHandler);
			this.SelectCommand = new RelayCommand(SelectHandler);
			this.DeleteCommand = new RelayCommand(DeleteHandler);
			this.LogoutCommand = new RelayCommand(LogoutHandler);
			//this.SelectionChangedCommand = new RelayCommand<object>((sender) => SelectionChangeHandler(sender));
		}

		#region RELAY COMMANDS
		public RelayCommand NewQuestionCommand { get; private set; }
		public RelayCommand SaveCommand { get; private set; }
		public RelayCommand SelectCommand { get; private set; }
		public RelayCommand DeleteCommand { get; private set; }
		public RelayCommand LogoutCommand { get; private set; }

		//public RelayCommand<object> SelectionChangedCommand { get; private set; }

		#endregion

		#region PROPERTIES
		/// <summary>
		/// The Selected Index property
		/// </summary>
		private int _selectedIndex;
		public int SelectedIndex {
			get { return _selectedIndex; }
			set {
				Set(() => this.SelectedIndex, ref _selectedIndex, value);
				SelectionChangeHandler(_selectedIndex);
			}
		}

		/// <summary>
		/// The Multiple Select property
		/// </summary>
		private bool _multipleSelect;
		public bool MultipleSelect {
			get { return _multipleSelect; }
			set { Set(() => this.MultipleSelect, ref _multipleSelect, value); }
		}

		/// <summary>
		/// The New Button visibility property
		/// </summary>
		private bool _newButtonVisibility;
		public bool NewButtonVisibility {
			get { return _newButtonVisibility; }
			set { Set(() => this.NewButtonVisibility, ref _newButtonVisibility, value); }
		}

		/// <summary>
		/// The Save Button visibility property
		/// </summary>
		private bool _saveButtonVisibility;
		public bool SaveButtonVisibility {
			get { return _saveButtonVisibility; }
			set { Set(() => this.SaveButtonVisibility, ref _saveButtonVisibility, value); }
		}

		/// <summary>
		/// The Select Button visibility property
		/// </summary>
		private bool _selectButtonVisibility;
		public bool SelectButtonVisibility {
			get { return _selectButtonVisibility; }
			set { Set(() => this.SelectButtonVisibility, ref _selectButtonVisibility, value); }
		}

		/// <summary>
		/// The Delete Button visibility property
		/// </summary>
		private bool _deleteButtonVisibility;
		public bool DeleteButtonVisibility {
			get { return _deleteButtonVisibility; }
			set { Set(() => this.DeleteButtonVisibility, ref _deleteButtonVisibility, value); }
		}
		#endregion

		private void SelectionChangeHandler(int index) {
			switch(index) {
				case 0:
					NewButtonVisibility = false;
					SaveButtonVisibility = true;
					SelectButtonVisibility = false;
					DeleteButtonVisibility = false;
				break;

				case 1:
					NewButtonVisibility = true;
					SaveButtonVisibility = false;
					SelectButtonVisibility = true;
					DeleteButtonVisibility = false;
				break;
			}
		}

		private void NewQuestionHandler() {
			try {
				// Tuple<UIElement, string> tupleValues = (Tuple<UIElement, string>)e.Parameter;
				NavigateTo(new Tuple<string, string>(PagesNames.AQuestionPage, "Test Params"));
			}
			catch (Exception e) {
				Debugger.Break();
			}
		}

		/// <summary>
		/// Save the Basic Information
		/// </summary>
		private void SaveHandler() {
			Messenger.Default.Send<GeneralMessages>(GeneralMessages.SAVE_INFO);
		}

		private void SelectHandler() {
			MultipleSelect = (MultipleSelect == false) ? true : false;
			SelectionModeChange message = new SelectionModeChange() {
				MultipleSelect = MultipleSelect
			};

			Messenger.Default.Send<SelectionModeChange>(message);
		}

		private void DeleteHandler() {

		}

		private void LogoutHandler() {
			this._navigationService.NavigateTo(PagesNames.LoginPage);
		}

		public void NavigateTo(Tuple<string, string> args) {
			this._navigationService.NavigateTo(args.Item1, args.Item1);
		}
	}
}
