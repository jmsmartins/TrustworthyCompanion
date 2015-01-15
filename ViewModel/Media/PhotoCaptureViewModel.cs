using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;

namespace TrustworthyCompanion.ViewModel.Media {
	public class PhotoCaptureViewModel : ViewModelBase {
		/// <summary>
		/// Initializes a new instance of the PhotoCaptureViewModel class.
		/// </summary>
		public PhotoCaptureViewModel() {
			this.PageLoadedCommand = new RelayCommand(PageLoaded);
		}

		#region RELAY COMMANDS
		public RelayCommand PageLoadedCommand { get; private set; }
		#endregion

		#region PROPERTIES
		/// <summary>
		/// The file source property
		/// </summary>
		private string _fileSource;
		public string FileSource{
			get { return _fileSource; }
			set { Set(() => this.FileSource, ref _fileSource, value); }
		}
		#endregion

		/// <summary>
		/// When the page loads, set properties
		/// </summary>
		private void PageLoaded() {
			
		}
	}
}