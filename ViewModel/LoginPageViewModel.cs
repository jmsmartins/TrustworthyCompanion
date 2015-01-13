using System;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Views;

namespace TrustworthyCompanion.ViewModel {
	/// <summary>
	/// This class contains properties that the main View can data bind to.
	/// <para>
	/// Use the <strong>mvvminpc</strong> snippet to add bindable properties to this ViewModel.
	/// </para>
	/// <para>
	/// You can also use Blend to data bind with the tool's support.
	/// </para>
	/// <para>
	/// See http://www.galasoft.ch/mvvm
	/// </para>
	/// </summary>
	public class LoginPageViewModel : ViewModelBase {

		// Navigation service
		private INavigationService _navigationService;

		/// <summary>
		/// Initializes a new instance of the LoginPageViewModel class.
		/// </summary>
		public LoginPageViewModel(INavigationService navigationService) {
			this._navigationService = navigationService;

			this.LoginCommand = new RelayCommand(() => LoginClickHandler(true));
			this.DetailsCommand = new RelayCommand<Tuple<string, string>>((args) => NavigateTo(args));

			// TODO - temporary data (to delete)
			this.Username = "admin";
			this.Password = "admin";
		}

		#region RELAY COMMANDS
		public RelayCommand<Tuple<string, string>> DetailsCommand { get; set; }
		public RelayCommand LoginCommand { get; private set; }
		#endregion

		#region PROPERTIES
		/// <summary>
		/// The Username property
		/// </summary>
		private string _username;
		public string Username {
			get { return _username; }
			set { Set(() => this.Username, ref _username, value); }
		}

		/// <summary>
		/// The Password property
		/// </summary>
		private string _password;
		public string Password {
			get { return _password; }
			set { Set(() => this.Password, ref _password, value); }
		}
		#endregion

		public void NavigateTo(Tuple<string, string> args) {
			this._navigationService.NavigateTo(args.Item1, args.Item1);
		}

		private void LoginClickHandler(bool value) {
			if(Username == "admin" && Password == "admin") {
				NavigateTo(new Tuple<string, string>("APivotPage", "Test Params"));
			} else {
				return;
			}
		}
	}
}