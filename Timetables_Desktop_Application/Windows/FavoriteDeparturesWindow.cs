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
	public partial class FavoriteDeparturesWindow : Form
	{
		/// <summary>
		/// Serializable class that serves only for load and save from and to the file.
		/// </summary>
		[Serializable]		
		public class FavoriteDepartures
		{
			/// <summary>
			/// One entry in the collection.
			/// </summary>
			[Serializable]
			public class FavoriteDeparture
			{
				/// <summary>
				/// Station.
				/// </summary>
				public string Station { get; set; }
				public override string ToString() => Station;
				public FavoriteDeparture(string station) => Station = station;
				internal FavoriteDeparture() { }
			}
			/// <summary>
			/// List of favorite departures.
			/// </summary>
			public List<FavoriteDeparture> Favorites { get; set; } = new List<FavoriteDeparture>();

			/// <summary>
			/// Remove all entries satisfying given criteria.
			/// </summary>
			/// <param name="station">Source stop.</param>
			public void Remove(string station) => Favorites.RemoveAll((FavoriteDeparture dep) => dep.Station == station);
			public void Remove(FavoriteDeparture dep) => Remove(dep.Station);
		}

		private FavoriteDepartures favorites;

		internal FavoriteDepartures departuresToFind = null;

		public FavoriteDeparturesWindow()
		{
			InitializeComponent();

			Settings.Theme.Apply(this);

			stationTextBox.Text = Settings.Localization.Station;
			addButton.Text = Settings.Localization.Add;
			removeButton.Text = Settings.Localization.Remove;
			findButton.Text = Settings.Localization.Find;
			
			// Try to load file with favorites, if exists.

			try
			{
				using (FileStream fileStream = new FileStream("departures.fav", FileMode.Open))
					favorites = (FavoriteDepartures)new XmlSerializer(typeof(FavoriteDepartures)).Deserialize(fileStream);
			}
			catch
			{
				favorites = new FavoriteDepartures();
			}

			foreach (var departure in favorites.Favorites)
				favoritesListBox.Items.Add(departure);
		}

		private void addButton_Click(object sender, System.EventArgs e)
		{
			var fav = new FavoriteDepartures.FavoriteDeparture(stationTextBox.Text);
			favoritesListBox.Items.Add(fav);
			favorites.Favorites.Add(fav);
		}

		private void FavoriteDeparturesWindow_FormClosing(object sender, FormClosingEventArgs e)
		{
			using (StreamWriter sw = new StreamWriter("departures.fav"))
				new XmlSerializer(typeof(FavoriteDepartures)).Serialize(sw, favorites);
		}

		private void removeButton_Click(object sender, EventArgs e)
		{
			foreach (FavoriteDepartures.FavoriteDeparture departuresToRemove in favoritesListBox.CheckedItems)
				favorites.Remove(departuresToRemove);

			favoritesListBox.Items.Clear();
			foreach (var departure in favorites.Favorites)
				favoritesListBox.Items.Add(departure);

			removeButton.Enabled = favoritesListBox.CheckedItems.Count > 0;
			findButton.Enabled = favoritesListBox.CheckedItems.Count > 0;
		}

		private void findButton_Click(object sender, EventArgs e)
		{
			departuresToFind = new FavoriteDepartures();

			foreach (FavoriteDepartures.FavoriteDeparture departure in favoritesListBox.CheckedItems)
				departuresToFind.Favorites.Add(departure);

			Close();
		}

		private void FavoriteDeparturesWindow_Load(object sender, EventArgs e)
		{
			Requests.AutoCompleteTextBox(stationTextBox, (string[])DataFeedDesktop.Basic.Stations);
		}

		private void favoritesListBox_SelectedIndexChanged(object sender, EventArgs e)
		{
			removeButton.Enabled = favoritesListBox.CheckedItems.Count > 0;
			findButton.Enabled = favoritesListBox.CheckedItems.Count > 0;
		}
	}
}
