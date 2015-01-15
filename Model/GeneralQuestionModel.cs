using GalaSoft.MvvmLight;

namespace TrustworthyCompanion.Model {
	/// <summary>
	/// This is the General question model class
	/// </summary>
	public class GeneralQuestionModel : ObservableObject {
		private string _id;
		/// <summary>
		/// Id property
		/// </summary>
		public string Id {
			get { return _id; }
			set { Set(() => Id, ref _id, value); }
		}

		private string _terms;
		/// <summary>
		/// Terms property
		/// </summary>
		public string Terms {
			get { return _terms; }
			set { Set(() => Terms, ref _terms, value); }
		}
		
	}
}
