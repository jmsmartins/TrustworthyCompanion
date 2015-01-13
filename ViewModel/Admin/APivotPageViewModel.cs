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
			//this.SelectionChangedCommand = new RelayCommand<object>((sender) => SelectionChangeHandler(sender));
		}

		#region RELAY COMMANDS
		public RelayCommand NewQuestionCommand { get; private set; }
		public RelayCommand SaveCommand { get; private set; }
		public RelayCommand SelectCommand { get; private set; }
		public RelayCommand DeleteCommand { get; private set; }

		//public RelayCommand<object> SelectionChangedCommand { get; private set; }

		#endregion

		#region PROPERTIES
		/// <summary>
		/// The Multiple Select property
		/// </summary>
		private bool multipleSelect;
		public bool MultipleSelect {
			get { return multipleSelect; }
			set {
				multipleSelect = value;
				RaisePropertyChanged(() => this.MultipleSelect);
			}
		}

		/// <summary>
		/// The New Button visibility property
		/// </summary>
		private bool newButtonVisibility;
		public bool NewButtonVisibility {
			get { return newButtonVisibility; }
			set {
				newButtonVisibility = value;
				RaisePropertyChanged(() => this.NewButtonVisibility);
			}
		}

		/// <summary>
		/// The Save Button visibility property
		/// </summary>
		private bool saveButtonVisibility;
		public bool SaveButtonVisibility {
			get { return saveButtonVisibility; }
			set {
				saveButtonVisibility = value;
				RaisePropertyChanged(() => this.SaveButtonVisibility);
			}
		}

		/// <summary>
		/// The Select Button visibility property
		/// </summary>
		private bool selectButtonVisibility;
		public bool SelectButtonVisibility {
			get { return selectButtonVisibility; }
			set {
				selectButtonVisibility = value;
				RaisePropertyChanged(() => this.SelectButtonVisibility);
			}
		}

		/// <summary>
		/// The Delete Button visibility property
		/// </summary>
		private bool deleteButtonVisibility;
		public bool DeleteButtonVisibility {
			get { return deleteButtonVisibility; }
			set {
				deleteButtonVisibility = value;
				RaisePropertyChanged(() => this.DeleteButtonVisibility);
			}
		}

		/// <summary>
		/// The Selected Index property
		/// </summary>
		private int selectedIndex;
		public int SelectedIndex {
			get { return selectedIndex; }
			set {
				selectedIndex = value;
				RaisePropertyChanged(() => this.SelectedIndex);
				SelectionChangeHandler(selectedIndex);
			}
		}
		#endregion

		private void SelectionChangeHandler(int index) {
			switch(index) {
				case 0:
					NewButtonVisibility = true;
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
				NavigateTo(new Tuple<string, string>("AQuestionPage", "Test Params"));
			}
			catch (Exception e) {
				Debugger.Break();
			}
		}

		private void SaveHandler() {

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

		public void NavigateTo(Tuple<string, string> args) {
			this._navigationService.NavigateTo(args.Item1, args.Item1);
		}
	}
}
