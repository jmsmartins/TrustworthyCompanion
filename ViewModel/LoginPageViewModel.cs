using System;
using System.Diagnostics;
using System.Threading.Tasks;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Views;
using TrustworthyCompanion.Database;
using TrustworthyCompanion.Tools;

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

		// Session
		private bool _hasSession = false;

		// Navigation service
		private INavigationService _navigationService;

		/// <summary>
		/// Initializes a new instance of the LoginPageViewModel class.
		/// </summary>
		public LoginPageViewModel(INavigationService navigationService) {
			this._navigationService = navigationService;
			// Set pages names
			PagesNames.SetPagesNames();

			this.PageLoadedCommand = new RelayCommand(PageLoaded);
			this.LoginCommand = new RelayCommand(LoginHandler);
			this.LoginAsGuestCommand = new RelayCommand(LoginAsGuestHandler);
			this.DetailsCommand = new RelayCommand<Tuple<string, string>>((args) => NavigateTo(args));
		}

		#region RELAY COMMANDS
		public RelayCommand PageLoadedCommand { get; private set; }
		public RelayCommand LoginCommand { get; private set; }
		public RelayCommand LoginAsGuestCommand { get; private set; }
		public RelayCommand<Tuple<string, string>> DetailsCommand { get; set; }
		
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

		private async void PageLoaded() {
			// TODO - temporary data (to delete)
			this.Username = "admin";
			this.Password = "admin";

			await Setup();

			// TODO Remove later on
			if(_hasSession) {
				LoginHandler();
			} else {
				LoginAsGuestHandler();
			}
		}

		public async Task Setup() {
			try {
				// Load the Database
				await DatabaseService.LoadDatabase();
			} catch(Exception e) {
				Debug.WriteLine(e.Message);
			}
		}

		public void NavigateTo(Tuple<string, string> args) {
			this._navigationService.NavigateTo(args.Item1, args.Item2);
		}

		private void LoginHandler() {
			if(Username == "admin" && Password == "admin") {
				_hasSession = true;
				NavigateTo(new Tuple<string, string>(PagesNames.ALandingPage, ""));
			} else {
				return;
			}
		}

		private void LoginAsGuestHandler() {
			NavigateTo(new Tuple<string, string>(PagesNames.USearchPage, ""));
		}
	}
}