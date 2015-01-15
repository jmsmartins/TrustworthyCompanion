using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Ioc;
using GalaSoft.MvvmLight.Messaging;
using TrustworthyCompanion.Model;
using Windows.UI.Xaml.Controls;

namespace TrustworthyCompanion.ViewModel.Media {
	public class VideoShowViewModel : ViewModelBase {

		/// <summary>
		/// Initializes a new instance of the VideoShowViewModel class.
		/// </summary>
		public VideoShowViewModel() {
			// Register the messenger
			// Subscribe the messenger for messages sent to it
			if(!SimpleIoc.Default.IsRegistered<QuestionModel>()) {
				Messenger.Default.Register<QuestionModel>(this, (action) => SetupProperties(action));
			}

			this.PageUnloadedCommand = new RelayCommand(PageUnloaded);
			this.PlayStopCommand = new RelayCommand<object>((sender) => PlayStopVideoHandler(sender));
		}

		#region RELAY COMMANDS
		public RelayCommand PageUnloadedCommand { get; private set; }
		public RelayCommand<object> PlayStopCommand { get; private set; }
		#endregion

		#region PROPERTIES
		/// <summary>
		/// The Question property
		/// </summary>
		private QuestionModel _question;
		public QuestionModel Question {
			get { return _question; }
			set { Set(() => this.Question, ref _question, value); }
		}
		#endregion

		private void SetupProperties(QuestionModel action) {
			Question = action;
		}

		/// <summary>
		/// When the page unloads
		/// </summary>
		private void PageUnloaded() {
			// Unregister the messenger to avoid receiving more messages when it's not open
			Messenger.Default.Unregister<QuestionModel>(this, (action) => SetupProperties(action));
		}

		private void PlayStopVideoHandler(object sender) {
			MediaElement element = (MediaElement)sender;
			if(element.CurrentState == Windows.UI.Xaml.Media.MediaElementState.Playing) {
				element.Stop();
			}
			else {
				element.Play();
			}
		}
	}
}