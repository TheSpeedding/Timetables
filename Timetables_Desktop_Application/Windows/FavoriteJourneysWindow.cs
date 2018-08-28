using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Linq;
using System.Windows.Forms;
using System.Xml.Serialization;
using Timetables.Client;

namespace Timetables.Application.Desktop
{
	public partial class FavoriteJourneysWindow : Form
	{
		/// <summary>
		/// Serializable class that serves only for load and save from and to the file.
		/// </summary>
		[Serializable]		
		public class FavoriteJourneys
		{
			/// <summary>
			/// One entry in the collection.
			/// </summary>
			[Serializable]
			public class FavoriteJourney
			{
				/// <summary>
				/// Source stop.
				/// </summary>
				public string Source { get; set; }
				/// <summary>
				/// Target stop.
				/// </summary>
				public string Target { get; set; }
				public override string ToString() => $"{ Source } - { Target }";
				public FavoriteJourney(string source, string target)
				{
					Source = source;
					Target = target;
				}
				internal FavoriteJourney() { }
			}
			/// <summary>
			/// List of favorite journeys (sources and targets).
			/// </summary>
			public List<FavoriteJourney> Favorites { get; set; } = new List<FavoriteJourney>();

			/// <summary>
			/// Remove all entries satisfying given criteria.
			/// </summary>
			/// <param name="source">Source stop.</param>
			/// <param name="target">Target stop.</param>
			public void Remove(string source, string target) => Favorites.RemoveAll((FavoriteJourney journey) => journey.Source == source && journey.Target == target);
			public void Remove(FavoriteJourney journey) => Remove(journey.Source, journey.Target);
		}

		private FavoriteJourneys favorites;

		internal FavoriteJourneys journeysToFind = null;

		public FavoriteJourneysWindow()
		{
			InitializeComponent();

			Settings.Theme.Apply(this);

			sourceComboBox.Text = Settings.Localization.SourceStop;
			targetComboBox.Text = Settings.Localization.TargetStop;
			addButton.Text = Settings.Localization.Add;
			removeButton.Text = Settings.Localization.Remove;
			findButton.Text = Settings.Localization.Find;

			foreach (var station in DataFeed.Basic.Stations)
			{
				sourceComboBox.Items.Add(station.Name);
				targetComboBox.Items.Add(station.Name);
			}

			// Try to load file with favorites, if exists.

			try
			{
				using (FileStream fileStream = new FileStream("journeys.fav", FileMode.Open))
					favorites = (FavoriteJourneys)new XmlSerializer(typeof(FavoriteJourneys)).Deserialize(fileStream);
			}
			catch
			{
				favorites = new FavoriteJourneys();
			}

			foreach (var journey in favorites.Favorites)
				favoritesListBox.Items.Add(journey);
		}

		private void addButton_Click(object sender, System.EventArgs e)
		{
			var fav = new FavoriteJourneys.FavoriteJourney(sourceComboBox.Text, targetComboBox.Text);
			favoritesListBox.Items.Add(fav);
			favorites.Favorites.Add(fav);
		}

		private void favoriteJourneysWindow_FormClosing(object sender, FormClosingEventArgs e)
		{
			using (StreamWriter sw = new StreamWriter("journeys.fav"))
				new XmlSerializer(typeof(FavoriteJourneys)).Serialize(sw, favorites);
		}

		private void removeButton_Click(object sender, EventArgs e)
		{
			foreach (FavoriteJourneys.FavoriteJourney journeyToRemove in favoritesListBox.CheckedItems)
				favorites.Remove(journeyToRemove);

			favoritesListBox.Items.Clear();
			foreach (var journey in favorites.Favorites)
				favoritesListBox.Items.Add(journey);
		}

		private void findButton_Click(object sender, EventArgs e)
		{
			journeysToFind = new FavoriteJourneys();

			foreach (FavoriteJourneys.FavoriteJourney journey in favoritesListBox.CheckedItems)
				journeysToFind.Favorites.Add(journey);

			Close();
		}
	}
}
