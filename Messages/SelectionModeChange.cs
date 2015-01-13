using GalaSoft.MvvmLight.Messaging;

namespace TrustworthyCompanion.Messages {

	public class SelectionModeChange : MessageBase {

		private bool multipleSelect;

		public bool MultipleSelect {
			get { return this.multipleSelect; }
			set { this.multipleSelect = value; }
		}
	}
}
