namespace TrustworthyCompanion.Tools {

	public static class PagesNames {
		public static string LoginPage { get; private set; }
		public static string APivotPage { get; private set; }
		public static string AQuestionPage { get; private set; }

		public static void SetPagesNames() {
			LoginPage = "LoginPage";
			APivotPage = "APivotPage";
			AQuestionPage = "AQuestionPage";
		}
	}
}