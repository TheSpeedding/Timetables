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
	public partial class FavoriteLinesWindow : Form
	{
		private IList<LineInfoCached> favorites;
		internal List<LineInfoCached> ItemsToFind { get; private set; }

		public FavoriteLinesWindow()
		{
			InitializeComponent();

			Settings.Theme.Apply(this);

			lineTextBox.Text = Settings.Localization.Line;
			addButton.Text = Settings.Localization.Add;
			removeButton.Text = Settings.Localization.Remove;
			findButton.Text = Settings.Localization.Find;

			favorites = LineInfoCached.FetchLineInfoData().ToList();			

			foreach (var item in favorites)
				favoritesListBox.Items.Add(item);
		}

		private void addButton_Click(object sender, System.EventArgs e)
		{
			Structures.Basic.RoutesInfoBasic.RouteInfoBasic route = Request.GetRouteInfoFromLabel(lineTextBox.Text);

			if (route == null || LineInfoCached.Select(route.ID) != null) return;

			var fav = new LineInfoCached(route.ID);

#pragma warning disable CS4014 // Because this call is not awaited, execution of the current method continues before the call is completed
			Request.CacheDepartureBoardAsync(fav.ConstructNewRequest());
#pragma warning restore CS4014 // Because this call is not awaited, execution of the current method continues before the call is completed

			favoritesListBox.Items.Add(fav);
			favorites.Add(fav);
		}

		private void removeButton_Click(object sender, EventArgs e)
		{
			foreach (LineInfoCached item in favoritesListBox.CheckedItems)
				item.Remove();

			favorites = LineInfoCached.FetchLineInfoData().ToList();

			favoritesListBox.Items.Clear();
			foreach (var item in favorites)
				favoritesListBox.Items.Add(item);

			removeButton.Enabled = favoritesListBox.CheckedItems.Count > 0;
			findButton.Enabled = favoritesListBox.CheckedItems.Count > 0;
		}

		private void findButton_Click(object sender, EventArgs e)
		{
			ItemsToFind = new List<LineInfoCached>();

			foreach (LineInfoCached item in favoritesListBox.CheckedItems)
				ItemsToFind.Add(item);

			Close();
		}

		private void FavoriteDeparturesWindow_Load(object sender, EventArgs e)
		{
			Request.AutoCompleteTextBox(lineTextBox, (string[])DataFeedDesktop.Basic.RoutesInfo);
		}

		private void favoritesListBox_SelectedIndexChanged(object sender, EventArgs e)
		{
			removeButton.Enabled = favoritesListBox.CheckedItems.Count > 0;
			findButton.Enabled = favoritesListBox.CheckedItems.Count > 0;
		}
	}
}
