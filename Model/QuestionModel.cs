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
