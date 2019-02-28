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
		private IList<JourneyCached> favorites;
		internal List<JourneyCached> ItemsToFind { get; private set; }

		public FavoriteJourneysWindow()
		{
			InitializeComponent();

			Settings.Theme.Apply(this);

			sourceTextBox.Text = Settings.Localization.SourceStop;
			targetTextBox.Text = Settings.Localization.TargetStop;
			addButton.Text = Settings.Localization.Add;
			removeButton.Text = Settings.Localization.Remove;
			findButton.Text = Settings.Localization.Find;

			favorites = JourneyCached.FetchJourneyData().ToList();

			foreach (var item in favorites)
				favoritesListBox.Items.Add(item);
		}

		private void addButton_Click(object sender, System.EventArgs e)
		{
			Structures.Basic.StationsBasic.StationBasic source = Request.GetStationFromString(sourceTextBox.Text);
			Structures.Basic.StationsBasic.StationBasic target = Request.GetStationFromString(targetTextBox.Text);

			if (source == null || target == null || JourneyCached.Select(source.ID, target.ID) != null) return;			

			var fav = new JourneyCached(source.ID, target.ID);

#pragma warning disable CS4014 // Because this call is not awaited, execution of the current method continues before the call is completed
			Request.CacheJourneyAsync(fav.ConstructNewRequest());
#pragma warning restore CS4014 // Because this call is not awaited, execution of the current method continues before the call is completed

			favoritesListBox.Items.Add(fav);
			favorites.Add(fav);
		}

		private void removeButton_Click(object sender, EventArgs e)
		{
			foreach (JourneyCached item in favoritesListBox.CheckedItems)
				item.Remove();

			favorites = JourneyCached.FetchJourneyData().ToList();

			favoritesListBox.Items.Clear();
			foreach (var item in favorites)
				favoritesListBox.Items.Add(item);

			removeButton.Enabled = favoritesListBox.CheckedItems.Count > 0;
			findButton.Enabled = favoritesListBox.CheckedItems.Count > 0;
		}

		private void findButton_Click(object sender, EventArgs e)
		{
			ItemsToFind = new List<JourneyCached>();

			foreach (JourneyCached item in favoritesListBox.CheckedItems)
				ItemsToFind.Add(item);

			Close();
		}

		private void FavoriteJourneysWindow_Load(object sender, EventArgs e)
		{
			Request.AutoCompleteTextBox(sourceTextBox, (string[])DataFeedDesktop.Basic.Stations);
			Request.AutoCompleteTextBox(targetTextBox, (string[])DataFeedDesktop.Basic.Stations);
		}

		private void favoritesListBox_SelectedIndexChanged(object sender, EventArgs e)
		{
			removeButton.Enabled = favoritesListBox.CheckedItems.Count > 0;
			findButton.Enabled = favoritesListBox.CheckedItems.Count > 0;
		}
	}
}
