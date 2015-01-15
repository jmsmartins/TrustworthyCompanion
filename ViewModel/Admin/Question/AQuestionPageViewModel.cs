using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Messaging;
using TrustworthyCompanion.Messages;

namespace TrustworthyCompanion.ViewModel.Admin.Question {
	public class AQuestionPageViewModel : ViewModelBase {

		/// <summary>
		/// Initializes a new instance of the AQuestionPageViewModel class.
		/// </summary>
		public AQuestionPageViewModel() {
			SelectedIndex = 0;

			this.SaveCommand = new RelayCommand(SaveHandler);
		}

		#region RELAY COMMANDS
		public RelayCommand SaveCommand { get; private set; }
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
		/// The Save Button visibility property
		/// </summary>
		private bool _saveButtonVisibility;
		public bool SaveButtonVisibility {
			get { return _saveButtonVisibility; }
			set { Set(() => this.SaveButtonVisibility, ref _saveButtonVisibility, value); }
		}
		#endregion

		private void SelectionChangeHandler(int index) {
			switch(index) {
				case 0: SaveButtonVisibility = true; break;
				case 1: SaveButtonVisibility = false; break;
			}
		}

		private void SaveHandler() {
			Messenger.Default.Send<GeneralMessages>(GeneralMessages.SAVE_QUESTION);
		}
	}
}
