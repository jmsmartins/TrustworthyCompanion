using GalaSoft.MvvmLight;

namespace TrustworthyCompanion.Model {
	/// <summary>
	/// This is the question model class
	/// </summary>
	public class BasicInformationModel : ObservableObject {
		private string _name;
		/// <summary>
		/// Name property
		/// </summary>
		public string Name {
			get { return _name; }
			set { Set(() => Name, ref _name, value); }
		}

		private string _phone;
		/// <summary>
		/// Phone property
		/// </summary>
		public string Phone {
			get { return _phone; }
			set { Set(() => Phone, ref _phone, value); }
		}

		private string _email;
		/// <summary>
		/// Email property
		/// </summary>
		public string Email {
			get { return _email; }
			set { Set(() => Email, ref _email, value); }
		}

		private string _address;
		/// <summary>
		/// Address property
		/// </summary>
		public string Address {
			get { return _address; }
			set { Set(() => Address, ref _address, value); }
		}
	}
}
