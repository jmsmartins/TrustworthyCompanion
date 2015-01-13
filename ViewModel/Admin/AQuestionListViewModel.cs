using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Ioc;
using GalaSoft.MvvmLight.Messaging;
using TrustworthyCompanion.Messages;

namespace TrustworthyCompanion.ViewModel.Admin {
	public class AQuestionListViewModel : ViewModelBase {
		/// <summary>
		/// Initializes a new instance of the AdminBasicInformationViewModel class.
		/// </summary>
		public AQuestionListViewModel() {
			InitViewModel();
		}

		#region PROPERTIES
		/// <summary>
		/// The ListView selection mode property
		/// </summary>
		private bool multipleSelect;
		public bool MultipleSelect {
			get { return multipleSelect; }
			set {
				multipleSelect = value;
				RaisePropertyChanged(() => this.MultipleSelect);
			}
		}
		#endregion

		private void InitViewModel() {
			// Subscribe the messenger for messages sent to it
			if(!SimpleIoc.Default.IsRegistered<SelectionModeChange>()) {
				Messenger.Default.Register<SelectionModeChange>(this, (action) => OnSelectionModeChange(action));
			}
		}


		private void OnSelectionModeChange(SelectionModeChange message) {
			MultipleSelect = message.MultipleSelect;
		}

	}
}
