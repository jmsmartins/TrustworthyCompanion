using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Ioc;
using GalaSoft.MvvmLight.Messaging;
using TrustworthyCompanion.Database;
using TrustworthyCompanion.Messages;
using TrustworthyCompanion.Model;

namespace TrustworthyCompanion.ViewModel.Admin {
	public class ABasicInformationViewModel : ViewModelBase {
		/// <summary>
		/// Initializes a new instance of the ABasicInformationViewModel class.
		/// </summary>
		public ABasicInformationViewModel() {
			this.ControlLoadedCommand = new RelayCommand(ControlLoaded);

		}

		#region RELAY COMMANDS
		public RelayCommand ControlLoadedCommand { get; private set; }
		#endregion

		#region PROPERTIES
		/// <summary>
		/// The Basic Information property
		/// </summary>
		private BasicInformationModel _basicInformation;
		public BasicInformationModel BasicInformation {
			get { return _basicInformation; }
			set { Set(() => this.BasicInformation, ref _basicInformation, value); }
		}
		#endregion

		/// <summary>
		/// When the control loads, set properties
		/// </summary>
		private async void ControlLoaded() {
			// Subscribe the messenger for messages sent to it
			if(!SimpleIoc.Default.IsRegistered<GeneralMessages>()) {
				Messenger.Default.Register<GeneralMessages>(this, (action) => SaveHandler(action));
			}

			await LoadData();
		}

		private async Task LoadData() {
			BasicInformation = await DatabaseService.GetBasicInformation();
		}

		private async void SaveHandler(GeneralMessages action) {
			if(action == GeneralMessages.SAVE_INFO) {
				await DatabaseService.UpdateBasicInformation(BasicInformation);
			} else {
				return;
			}
		}
	}
}
