using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GalaSoft.MvvmLight;

namespace TrustworthyCompanion.Model {
	/// <summary>
	/// This is the media model class
	/// </summary>
	public class MediaModel : ObservableObject {
		private string _name;
		/// <summary>
		/// Name property
		/// </summary>
		public string Name {
			get { return _name; }
			set { Set(() => Name, ref _name, value); }
		}

	}
}
