namespace TrustworthyCompanion.Tools {

	public static class PagesNames {
		public static string LoginPage { get; private set; }
		public static string APivotPage { get; private set; }
		public static string AQuestionPage { get; private set; }
		public static string PhotoCapturePage { get; private set; }
		public static string VideoCapturePage { get; private set; }
		public static string AudioCapturePage { get; private set; }

		public static void SetPagesNames() {
			LoginPage = "LoginPage";
			APivotPage = "APivotPage";
			AQuestionPage = "AQuestionPage";
			PhotoCapturePage = "PhotoCapturePage";
			VideoCapturePage = "VideoCapturePage";
			AudioCapturePage = "AudioCapturePage";
		}
	}
}