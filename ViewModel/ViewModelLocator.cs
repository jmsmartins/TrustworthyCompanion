/*
  In App.xaml:
  <Application.Resources>
      <vm:ViewModelLocator xmlns:vm="clr-namespace:TrustworthyCompanion"
                           x:Key="Locator" />
  </Application.Resources>
  
  In the View:
  DataContext="{Binding Source={StaticResource Locator}, Path=ViewModelName}"

  You can also use Blend to do all this with the tool's support.
  See http://www.galasoft.ch/mvvm
*/

using GalaSoft.MvvmLight.Ioc;
using GalaSoft.MvvmLight.Views;
using Microsoft.Practices.ServiceLocation;
using TrustworthyCompanion.View;
using TrustworthyCompanion.View.Admin;
using TrustworthyCompanion.View.Admin.Question;
using TrustworthyCompanion.View.Media;
using TrustworthyCompanion.View.User;
using TrustworthyCompanion.View.User.Question;
using TrustworthyCompanion.ViewModel.Admin;
using TrustworthyCompanion.ViewModel.Admin.Question;
using TrustworthyCompanion.ViewModel.Media;
using TrustworthyCompanion.ViewModel.User;
using TrustworthyCompanion.ViewModel.User.Question;

namespace TrustworthyCompanion.ViewModel {
	/// <summary>
	/// This class contains static references to all the view models in the
	/// application and provides an entry point for the bindings.
	/// </summary>
	public class ViewModelLocator {
		/// <summary>
		/// Initializes a new instance of the ViewModelLocator class.
		/// </summary>
		public ViewModelLocator() {
			ServiceLocator.SetLocatorProvider(() => SimpleIoc.Default);

			var navigationService = this.CreateNavigationService();
			SimpleIoc.Default.Register<INavigationService>(() => navigationService);
			SimpleIoc.Default.Register<IDialogService, DialogService>();

			SimpleIoc.Default.Register<LoginPageViewModel>();

			// Admin Pages
			SimpleIoc.Default.Register<ALandingPageViewModel>();
			SimpleIoc.Default.Register<AQuestionPageViewModel>();

			// Admin User Controls
			SimpleIoc.Default.Register<ABasicInformationViewModel>();
			SimpleIoc.Default.Register<AQuestionListViewModel>();
			SimpleIoc.Default.Register<AQuestionViewModel>();
			SimpleIoc.Default.Register<AQuestionMediaViewModel>();

			// User Pages
			SimpleIoc.Default.Register<USearchPageViewModel>();
			SimpleIoc.Default.Register<ULandingPageViewModel>();
			SimpleIoc.Default.Register<UQuestionPageViewModel>();

			// User User Controls
			SimpleIoc.Default.Register<UQuestionViewModel>();
			SimpleIoc.Default.Register<UQuestionMediaViewModel>();

			// Media Pages
			SimpleIoc.Default.Register<PhotoCaptureViewModel>();
			SimpleIoc.Default.Register<VideoCaptureViewModel>();
			SimpleIoc.Default.Register<AudioCaptureViewModel>();
			SimpleIoc.Default.Register<PhotoShowViewModel>();
			SimpleIoc.Default.Register<VideoShowViewModel>();
			SimpleIoc.Default.Register<AudioShowViewModel>();
		}

		private INavigationService CreateNavigationService() {
			var navigationService = new NavigationService();

			navigationService.Configure("LoginPage", typeof(LoginPage));

			// Admin Pages
			navigationService.Configure("ALandingPage", typeof(ALandingPage));
			navigationService.Configure("AQuestionPage", typeof(AQuestionPage));

			// User Pages
			navigationService.Configure("USearchPage", typeof(USearchPage));
			navigationService.Configure("ULandingPage", typeof(ULandingPage));
			navigationService.Configure("UQuestionPage", typeof(UQuestionPage));

			// Media
			navigationService.Configure("PhotoCapturePage", typeof(PhotoCapturePage));
			navigationService.Configure("VideoCapturePage", typeof(VideoCapturePage));
			navigationService.Configure("AudioCapturePage", typeof(AudioCapturePage));

			navigationService.Configure("PhotoShowPage", typeof(PhotoShowPage));
			navigationService.Configure("VideoShowPage", typeof(VideoShowPage));
			navigationService.Configure("AudioShowPage", typeof(AudioShowPage));

			return navigationService;
		}

		public LoginPageViewModel LoginPageViewModel {
			get { return ServiceLocator.Current.GetInstance<LoginPageViewModel>(); }
		}

		# region ADMIN PAGES
		public ALandingPageViewModel APivotPageViewModel {
			get { return ServiceLocator.Current.GetInstance<ALandingPageViewModel>();	}
		}

		public AQuestionPageViewModel AQuestionPageViewModel {
			get { return ServiceLocator.Current.GetInstance<AQuestionPageViewModel>(); }
		}
		#endregion

		#region ADMIN USER CONTROLS
		public AQuestionViewModel AQuestionViewModel {
			get { return ServiceLocator.Current.GetInstance<AQuestionViewModel>(); }
		}

		public AQuestionMediaViewModel AQuestionMediaViewModel {
			get { return ServiceLocator.Current.GetInstance<AQuestionMediaViewModel>(); }
		}

		public ABasicInformationViewModel ABasicInformationViewModel {
			get { return ServiceLocator.Current.GetInstance<ABasicInformationViewModel>(); }
		}

		public AQuestionListViewModel AQuestionListViewModel {
			get { return ServiceLocator.Current.GetInstance<AQuestionListViewModel>(); }
		}
		#endregion

		#region USER PAGES
		public USearchPageViewModel USearchPageViewModel {
			get { return ServiceLocator.Current.GetInstance<USearchPageViewModel>(); }
		}

		public ULandingPageViewModel UPivotPageViewModel {
			get { return ServiceLocator.Current.GetInstance<ULandingPageViewModel>(); }
		}

		public UQuestionPageViewModel UQuestionPageViewModel {
			get { return ServiceLocator.Current.GetInstance<UQuestionPageViewModel>(); }
		}
		#endregion

		#region USER USER CONTROLS
		public UQuestionViewModel UQuestionViewModel {
			get { return ServiceLocator.Current.GetInstance<UQuestionViewModel>(); }
		}

		public UQuestionMediaViewModel UQuestionMediaViewModel {
			get { return ServiceLocator.Current.GetInstance<UQuestionMediaViewModel>(); }
		}
		#endregion

		#region MEDIA
		public PhotoCaptureViewModel PhotoCaptureViewModel {
			get { return ServiceLocator.Current.GetInstance<PhotoCaptureViewModel>(); }
		}

		public VideoCaptureViewModel VideoCaptureViewModel {
			get { return ServiceLocator.Current.GetInstance<VideoCaptureViewModel>(); }
		}

		public AudioCaptureViewModel AudioCaptureViewModel {
			get { return ServiceLocator.Current.GetInstance<AudioCaptureViewModel>(); }
		}

		public PhotoShowViewModel PhotoShowViewModel {
			get { return ServiceLocator.Current.GetInstance<PhotoShowViewModel>(); }
		}

		public VideoShowViewModel VideoShowViewModel {
			get { return ServiceLocator.Current.GetInstance<VideoShowViewModel>(); }
		}

		public AudioShowViewModel AudioShowViewModel {
			get { return ServiceLocator.Current.GetInstance<AudioShowViewModel>(); }
		}
		#endregion

		public static void Cleanup() {
			// TODO Clear the ViewModels
		}
	}
}