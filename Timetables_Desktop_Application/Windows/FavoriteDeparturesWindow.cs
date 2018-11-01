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
		private IList<StationInfoCached> favorites;
		internal List<StationInfoCached> ItemsToFind { get; private set; }

		public FavoriteDeparturesWindow()
		{
			InitializeComponent();

			Settings.Theme.Apply(this);

			stationTextBox.Text = Settings.Localization.Station;
			addButton.Text = Settings.Localization.Add;
			removeButton.Text = Settings.Localization.Remove;
			findButton.Text = Settings.Localization.Find;

			favorites = StationInfoCached.FetchStationInfoData().ToList();			

			foreach (var item in favorites)
				favoritesListBox.Items.Add(item);
		}

		private void addButton_Click(object sender, System.EventArgs e)
		{
			Structures.Basic.StationsBasic.StationBasic station = Requests.GetStationFromString(stationTextBox.Text);

			if (station == null) return;

			var fav = new StationInfoCached(station.ID);

#pragma warning disable CS4014 // Because this call is not awaited, execution of the current method continues before the call is completed
			Requests.CacheDepartureBoardAsync(fav.ConstructNewRequest());
#pragma warning restore CS4014 // Because this call is not awaited, execution of the current method continues before the call is completed

			favoritesListBox.Items.Add(fav);
			favorites.Add(fav);
		}

		private void removeButton_Click(object sender, EventArgs e)
		{
			foreach (StationInfoCached item in favoritesListBox.CheckedItems)
				item.Remove();

			favorites = StationInfoCached.FetchStationInfoData().ToList();

			favoritesListBox.Items.Clear();
			foreach (var item in favorites)
				favoritesListBox.Items.Add(item);

			removeButton.Enabled = favoritesListBox.CheckedItems.Count > 0;
			findButton.Enabled = favoritesListBox.CheckedItems.Count > 0;
		}

		private void findButton_Click(object sender, EventArgs e)
		{
			ItemsToFind = new List<StationInfoCached>();

			foreach (StationInfoCached item in favoritesListBox.CheckedItems)
				ItemsToFind.Add(item);

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
