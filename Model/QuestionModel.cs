using GalaSoft.MvvmLight;

namespace TrustworthyCompanion.Model {
	/// <summary>
	/// This is the question model class
	/// </summary>
	public class QuestionModel : ObservableObject {
		private int _id;
		/// <summary>
		/// Question ID property
		/// </summary>
		public int Id {
			get { return _id; }
			set { Set(() => Id, ref _id, value); }
		}

		private string _title;
		/// <summary>
		/// Question title property
		/// </summary>
		public string Title {
			get { return _title; }
			set { Set(() => Title, ref _title, value); }
		}

		private string _message;
		/// <summary>
		/// Question message property
		/// </summary>
		public string Message {
			get { return _message; }
			set { Set(() => Message, ref _message, value); }
		}

		private string _audioFile;
		/// <summary>
		/// Audio File path property
		/// </summary>
		public string AudioFile {
			get { return _audioFile; }
			set { Set(() => AudioFile, ref _audioFile, value); }
		}

		private string _photoFile;
		/// <summary>
		/// Photo File path property
		/// </summary>
		public string PhotoFile {
			get { return _photoFile; }
			set { Set(() => PhotoFile, ref _photoFile, value); }
		}

		private string _videoFile;
		/// <summary>
		/// Video File path property
		/// </summary>
		public string VideoFile {
			get { return _videoFile; }
			set { Set(() => VideoFile, ref _videoFile, value); }
		}

		private bool _isSelected;
		/// <summary>
		/// Is Selected property
		/// </summary>
		public bool IsSelected {
			get { return _isSelected; }
			set { Set(() => IsSelected, ref _isSelected, value); }
		}
	
	}
}
