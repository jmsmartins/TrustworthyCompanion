using System;
using System.Diagnostics;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Views;
using TrustworthyCompanion.View.Admin;

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
		}

		#region RELAY COMMANDS
		public RelayCommand<Tuple<string, string>> DetailsCommand { get; set; }
		public RelayCommand LoginCommand { get; private set; }
		#endregion

		/// <summary>
		/// The notification title
		/// </summary>
		private string title;
		public string Title {
			get { return title; }
			set {
				title = value;
				RaisePropertyChanged(() => this.Title);
			}
		}

		#region PROPERTIES
		/// <summary>
		/// The Username property
		/// </summary>
		private string username;
		public string Username {
			get { return username; }
			set {
				username = value;
				RaisePropertyChanged(() => this.Username);
			}
		}

		/// <summary>
		/// The Password property
		/// </summary>
		private string password;
		public string Password {
			get { return password; }
			set {
				password = value;
				RaisePropertyChanged(() => this.Password);
			}
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