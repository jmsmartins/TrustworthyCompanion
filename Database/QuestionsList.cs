using GalaSoft.MvvmLight;
using SQLite;

namespace TrustworthyCompanion.Database {
	/// <summary>
	/// QuestionsList table
	/// </summary>
	public class QuestionsList : ObservableObject {
		private int _id;
		/// <summary>
		/// Question ID property
		/// </summary>
		[PrimaryKey, AutoIncrement]
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
	}
}