namespace TrustworthyCompanion.Tools {

	public static class PagesNames {
		public static string LoginPage { get; private set; }

		// Admin pages
		public static string ALandingPage { get; private set; }
		public static string AQuestionPage { get; private set; }

		// User pages
		public static string USearchPage { get; private set; }
		public static string ULandingPage { get; private set; }
		public static string UQuestionPage { get; private set; }
		public static string MapPage { get; private set; }

		// Media Pages
		public static string PhotoCapturePage { get; private set; }
		public static string VideoCapturePage { get; private set; }
		public static string AudioCapturePage { get; private set; }
		public static string PhotoShowPage { get; private set; }
		public static string VideoShowPage { get; private set; }
		public static string AudioShowPage { get; private set; }

		public static void SetPagesNames() {
			LoginPage = "LoginPage";

			// Admin pages
			ALandingPage = "ALandingPage";
			AQuestionPage = "AQuestionPage";

			// User pages
			USearchPage = "USearchPage";
			ULandingPage = "ULandingPage";
			UQuestionPage = "UQuestionPage";
			MapPage = "MapPage";

			// Media Pages
			PhotoCapturePage = "PhotoCapturePage";
			VideoCapturePage = "VideoCapturePage";
			AudioCapturePage = "AudioCapturePage";
			PhotoShowPage = "PhotoShowPage";
			VideoShowPage = "VideoShowPage";
			AudioShowPage = "AudioShowPage";
		}
	}
}