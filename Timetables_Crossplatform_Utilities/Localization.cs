using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace Timetables.Client
{
	/// <summary>
	/// Class offering localized string constants.
	/// </summary>
	[Serializable]
	public class Localization
	{
		[XmlIgnore]
		private string locName = "English";
		public string Loading { get; set; } = "Loading...";
		public string UseCellularsToCache { get; set; } = "Use cellular data to cache";
		public string TransfersSpeed { get; set; } = "Transfers speed";
		public string AlreadyCached { get; set; } = "This item is already cached.";
		public string MainMenu { get; set; } = "Main menu";
		public string Journey { get; set; } = "Journey";
		public string FindInformationAboutLine { get; set; } = "Find information about line";
		public string FindJourney { get; set; } = "Find journey";
		public string DepartureBoard { get; set; } = "Departure board";
		public string FindDeparturesFromTheStation { get; set; } = "Find departures from the station";
		public string Traffic { get; set; } = "Traffic";
		public string Lockouts { get; set; } = "Lockouts";
		public string ExtraordinaryEvents { get; set; } = "Extraordinary events";
		public string Settings { get; set; } = "Settings";
		public string Theme { get; set; } = "Theme";
		public string DarkTheme { get; set; } = "Dark";
		public string LightTheme { get; set; } = "Light";
		public string BlueTheme { get; set; } = "Blue";
		public string Language { get; set; } = "Language";
		public string ShowMap { get; set; } = "Show map";
		public string FavoriteJourneys { get; set; } = "Favorite journeys";
		public string ProblemWhileLoadingSettings { get; set; } = "Problem occured while loading user settings. Set the application up using following dialog.";
		public string RestartNeeded { get; set; } = "Restart needed";
		public string RestartToApplyChanges { get; set; } = "You have to restart the application to apply the changes.";
		public string SourceStop { get; set; } = "Source";
		public string TargetStop { get; set; } = "Target";
		public string TransfersCount { get; set; } = "Transfers";
		public string LeavingTime { get; set; } = "Departure";
		public string Count { get; set; } = "Count";
		public string Search { get; set; } = "Search";
		public string Station { get; set; } = "Station";
		public string Departure { get; set; } = "Departure";
		public string Departures { get; set; } = "Departures";
		public string Journeys { get; set; } = "Journeys";
		public string NoTransfers { get; set; } = "No transfer";
		public string OneTransfer { get; set; } = "1 transfer";
		public string Transfers { get; set; } = "transfers";
		public string Minute { get; set; } = "minute";
		public string Minutes { get; set; } = "minutes";
		public string Hour { get; set; } = "hour";
		public string Hours { get; set; } = "hours";
		public string Day { get; set; } = "day";
		public string Days { get; set; } = "days";
		public string Left { get; set; } = "Left";
		public string Ago { get; set; } = "ago";
		public string LeavesIn { get; set; } = "Leaves in";
		public string NewStationInfo { get; set; } = "New station info";
		public string NewLineInfo { get; set; } = "New line info";
		public string NewJourney { get; set; } = "New journey";
		public string StationNotFound { get; set; } = "Station not found";
		public string UnableToFindStation { get; set; } = "Unable to find station with name";
		public string Map { get; set; } = "Map";
		public string Detail { get; set; } = "Detail";
		public string Print { get; set; } = "Print";
		public string Transfer { get; set; } = "Transfer";
		public string Outdated { get; set; } = "Outdated!";
		public string UnreachableHost { get; set; } = "The server is unreachable. It might be offline, try again later.";
		public string Offline { get; set; } = "Offline";
		public string Add { get; set; } = "Add";
		public string Remove { get; set; } = "Remove";
		public string Find { get; set; } = "Find";
		public string AddToFavorites { get; set; } = "Add to favorites";
		public string PleaseWaitDownloading { get; set; } = "Please wait while data are just being downloaded.";
		public string WalkingSpeed { get; set; } = "W. speed";
		public string SlowSpeed { get; set; } = "Slow";
		public string MediumSpeed { get; set; } = "Medium";
		public string FastSpeed { get; set; } = "Fast";
		public string UnableToFindRouteInfo { get; set; } = "Unable to find route with label";
		public string RouteInfoNotFound { get; set; } = "Route not found";
		public string Line { get; set; } = "Line";
		public string Sort { get; set; } = "Sort";
		public string DepartureTimeAscending { get; set; } = "By departure time ascending";
		public string ArrivalTimeAscending { get; set; } = "By arrival time ascending";
		public string TransfersCountAscending { get; set; } = "By transfers count ascending";
		public string WaitingTimesAscending { get; set; } = "By waiting times ascending";
		public string ArrivalAt { get; set; } = "Arrival at";
		public string NoDeparturesFromThisStop { get; set; } = "There are no departures from this stop.";
		public string EditParameters { get; set; } = "Edit parameters";
		public string PrintList { get; set; } = "Print list";
		public string MeanOfTransport { get; set; } = "Type";
		public string Subway { get; set; } = "Subway";
		public string Tram { get; set; } = "Tram";
		public string Bus { get; set; } = "Bus";
		public string Train { get; set; } = "Train";
		public string Cablecar { get; set; } = "Cablecar";
		public string Ship { get; set; } = "Ship";
		public string ForceDataDownload { get; set; } = "Force data download";
		public string DataDownloadedSuccessfully { get; set; } = "Data downloaded successfully.";
		public string Headsign { get; set; } = "Headsign";
		public string GetLineInfo { get; set; } = "Get information about line";
		public string ForceCachedDataUpdate { get; set; } = "Update cache";
		public string FavoriteLines { get; set; } = "Favorite lines";
		public string FavoriteStations { get; set; } = "Favorite stations";
			
		/// <summary>
		/// Returns language that is represented by the object.
		/// </summary>
		/// <returns></returns>
		public override string ToString() => locName;
		/// <summary>
		/// Creates a new instance of Localization class offering string constants in given language.
		/// Uses filesystem methods.
		/// </summary>
		/// <param name="language">Language.</param>
		/// <returns>Localization object.</returns>
		public static Localization GetTranslation(string language = "English")
		{
			if (language == "English") return new Localization();			
			
			if (!File.Exists("loc/" + language + ".xml")) throw new ArgumentException($"Translation sheet with language \" { language } \" was not found.");
			
			using (FileStream fileStream = new FileStream("loc/" + language + ".xml", FileMode.Open))
			{
				try
				{
					var loc = (Localization)new XmlSerializer(typeof(Localization)).Deserialize(fileStream);
					loc.locName = language;
					return loc;
				}
				catch
				{
					return new Localization();
				}
			}
		}
		/// <summary>
		/// Creates a new instance of Localization class offering string constants in given language.
		/// Uses stream methods, used in Android application.
		/// </summary>
		/// <param name="info">Tuple to store a stream and name of the language.</param>
		/// <returns>Localization object.</returns>
		public static Localization GetTranslation(Tuple<Stream, string> info = null)
		{
			if (info == null || info.Item2 == "English") return new Localization();

			try
			{
				var loc = (Localization)new XmlSerializer(typeof(Localization)).Deserialize(info.Item1);
				loc.locName = info.Item2;
				return loc;
			}
			catch
			{
				return new Localization();
			}

		}
	}
}
