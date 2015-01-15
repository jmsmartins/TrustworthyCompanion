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
using TrustworthyCompanion.ViewModel.Admin;
using TrustworthyCompanion.ViewModel.Admin.Question;
using TrustworthyCompanion.ViewModel.Media;

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

			// Pages
			SimpleIoc.Default.Register<LoginPageViewModel>();
			SimpleIoc.Default.Register<APivotPageViewModel>();
			SimpleIoc.Default.Register<AQuestionPageViewModel>();

			// Media Pages
			SimpleIoc.Default.Register<PhotoCaptureViewModel>();
			SimpleIoc.Default.Register<VideoCaptureViewModel>();
			SimpleIoc.Default.Register<AudioCaptureViewModel>();

			// User Controls
			SimpleIoc.Default.Register<ABasicInformationViewModel>();
			SimpleIoc.Default.Register<AQuestionListViewModel>();
			SimpleIoc.Default.Register<AQuestionViewModel>();
			SimpleIoc.Default.Register<AQuestionMediaViewModel>();
		}

		private INavigationService CreateNavigationService() {
			var navigationService = new NavigationService();
			// Pages
			navigationService.Configure("LoginPage", typeof(LoginPage));
			navigationService.Configure("APivotPage", typeof(APivotPage));
			navigationService.Configure("AQuestionPage", typeof(AQuestionPage));

			// Media
			navigationService.Configure("PhotoCapturePage", typeof(PhotoCapturePage));
			navigationService.Configure("VideoCapturePage", typeof(VideoCapturePage));
			navigationService.Configure("AudioCapturePage", typeof(AudioCapturePage));

			return navigationService;
		}

		# region PAGES
		public LoginPageViewModel LoginPageViewModel {
			get { return ServiceLocator.Current.GetInstance<LoginPageViewModel>(); }
		}

		public APivotPageViewModel APivotPageViewModel {
			get { return ServiceLocator.Current.GetInstance<APivotPageViewModel>();	}
		}

		public AQuestionPageViewModel AQuestionPageViewModel {
			get { return ServiceLocator.Current.GetInstance<AQuestionPageViewModel>(); }
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
		#endregion

		#region USER CONTROLS
		public AQuestionViewModel AQuestionViewModel {
			get { return ServiceLocator.Current.GetInstance<AQuestionViewModel>(); }
		}

		public AQuestionMediaViewModel AQuestionMediaViewModel {
			get { return ServiceLocator.Current.GetInstance<AQuestionMediaViewModel>();	}
		}

		public ABasicInformationViewModel ABasicInformationViewModel {
			get { return ServiceLocator.Current.GetInstance<ABasicInformationViewModel>(); }
		}

		public AQuestionListViewModel AQuestionListViewModel {
			get { return ServiceLocator.Current.GetInstance<AQuestionListViewModel>(); }
		}
		#endregion

		public static void Cleanup() {
			// TODO Clear the ViewModels
		}
	}
}