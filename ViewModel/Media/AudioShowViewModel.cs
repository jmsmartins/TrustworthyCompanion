using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Ioc;
using GalaSoft.MvvmLight.Messaging;
using TrustworthyCompanion.Model;
using Windows.UI.Xaml.Controls;

namespace TrustworthyCompanion.ViewModel.Media {
	public class AudioShowViewModel : ViewModelBase {
		/// <summary>
		/// Initializes a new instance of the AudioShowViewModel class.
		/// </summary>
		public AudioShowViewModel() {
			// Register the messenger
			// Subscribe the messenger for messages sent to it
			if(!SimpleIoc.Default.IsRegistered<QuestionModel>()) {
				Messenger.Default.Register<QuestionModel>(this, (action) => SetupProperties(action));
			}

			this.PageUnloadedCommand = new RelayCommand(PageUnloaded);
			this.PlayAudioCommand = new RelayCommand<object>((item) => PlayAudioHandler(item));
		}

		#region RELAY COMMANDS
		public RelayCommand PageUnloadedCommand { get; private set; }
		public RelayCommand<object> PlayAudioCommand { get; private set; }
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

		private void PlayAudioHandler(object sender) {
			MediaElement element = (MediaElement)sender;
			if(element.CurrentState == Windows.UI.Xaml.Media.MediaElementState.Playing) {
				element.Stop();
			} else {
				element.Play();
			}
		}
	}
}
