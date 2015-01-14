using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
using System.Threading.Tasks;
using SQLite;
using TrustworthyCompanion.Model;
using Windows.Storage;

namespace TrustworthyCompanion.Database {
	public static class DatabaseService {

		private static string _database = "TrustworthyCompanion.db";
		private static bool _hasDatabase = true;

		/// <summary>
		/// Creates or Loads the Database
		/// </summary>
		/// <param name="appName">The name of the Main Application</param>
		/// <returns>Task completed</returns>
		public static async Task LoadDatabase() {
			//no exception means file exists
			try {
				var file = await ApplicationData.Current.LocalFolder.GetFileAsync(_database);
				_hasDatabase = true;
			} catch(FileNotFoundException) {
				_hasDatabase = false;
			}

			if(!_hasDatabase) {
				string error = String.Empty;
				try {
					using(SQLiteAsyncConnection sqlConnection = new SQLiteAsyncConnection(_database)) {
						// QUESTIONS_LIST TABLE
						// To keep record of all questions
						await sqlConnection.CreateTableAsync<QuestionsList>();

						// BASIC_INFORMATION TABLE
						// To keep record of basic information
						await sqlConnection.CreateTableAsync<BasicInformation>();
					}

					// Insert the default data
					//await InsertDefaultData(appName, appVersion, clientId, clientSecret, context, deviceToken, senderId, publisherId, channelUri);
					// Insert dummy data
					await DatabaseService.InsertDummyData();
				} catch(Exception e) {
					error = e.Message;
				}
			} else {
				// Do something
			}
		}

		# region BASIC INFORMATION
		/// <summary>
		/// Gets the Basic Information
		/// </summary>
		public static async Task<BasicInformationModel> GetBasicInformation() {
			BasicInformationModel info = null;
			using(SQLiteAsyncConnection sqlConnection = new SQLiteAsyncConnection(_database)) {
				await sqlConnection.Table<BasicInformation>().FirstAsync().ContinueWith((t) => {
					if(t.Exception == null) {
						info = new BasicInformationModel() {
							Name = t.Result.Name,
							Phone = t.Result.Phone,
							Email = t.Result.Email,
							Address = t.Result.Address
						};
					}
				});
			}
			return info;
		}

		/// <summary>
		/// Updates the Basic Information to the database
		/// </summary>
		/// <param name="info">The info data</param>
		/// <returns>Task completed</returns>
		public static async Task UpdateBasicInformation(BasicInformationModel info) {
			try {
				using(SQLiteAsyncConnection sqlConnection = new SQLiteAsyncConnection(_database)) {
					await sqlConnection.QueryAsync<BasicInformation>("UPDATE BasicInformation SET Name = ?, Phone = ?, Email = ?, Address = ?", info.Name, info.Phone, info.Email, info.Address);
				}
			} catch(SQLiteException e) {
				Debug.WriteLine(e.Message);
			}
		}
		#endregion

		/// <summary>
		/// Returns a collection of questions
		/// </summary>
		/// <returns>The collection of questions</returns>
		public static async Task<ObservableCollection<QuestionModel>> GetQuestions() {
			ObservableCollection<QuestionModel> questionsList = new ObservableCollection<QuestionModel>();

			using(SQLiteAsyncConnection sqlConnection = new SQLiteAsyncConnection(_database)) {
				await sqlConnection.QueryAsync<QuestionsList>("SELECT id, title FROM QuestionsList").ContinueWith((t) => {
					if(t.Exception == null) {
						foreach(var item in t.Result) {
							QuestionModel notification = new QuestionModel() {
								Id = item.Id,
								Title = item.Title
							};
							questionsList.Add(notification);
						}
					}
				});
			}
			return questionsList;
		}

		# region INSERT DUMMY DATA (TO REMOVE)
		public async static Task InsertDummyData() {
			try {

				// Questions
				using(SQLiteAsyncConnection sqlConnection = new SQLiteAsyncConnection(_database)) {
					// Basic Information
					BasicInformation info = new BasicInformation() {
						Name = "John Doe",
						Phone = "+351911520974",
						Email = "trustworthycompanion@mailinator.com",
						Address = "Rua Dr. António Bernardino de Almeida, 431, Porto"
					};

					await sqlConnection.InsertAsync(info);

					// Questions
					for(int i = 1; i <= 10; i++) {
						QuestionsList notificationsList = new QuestionsList() {
							Title = "Question " + i.ToString()
						};

						await sqlConnection.InsertAsync(notificationsList);
					}
				}
			} catch(SQLiteException e) {
				Debug.WriteLine("ERROR: " + e.Message);
			}
		}
		# endregion
	}
}
