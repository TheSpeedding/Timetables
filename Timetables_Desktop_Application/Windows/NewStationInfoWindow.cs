﻿using System;
using System.Windows.Forms;
using WeifenLuo.WinFormsUI.Docking;
using Timetables.Client;
using System.Linq;
using System.Threading.Tasks;
using Timetables.Utilities;

namespace Timetables.Application.Desktop
{
	public partial class NewStationInfoWindow : DockContent
	{
		public NewStationInfoWindow(string station = "")
		{
			InitializeComponent();
			Settings.Theme.Apply(this);

			Text = Settings.Localization.NewStationInfo;

			countLabel.Text = Settings.Localization.Count;
			departureLabel.Text = Settings.Localization.LeavingTime;
			stationLabel.Text = Settings.Localization.Station;
			lineLabel.Text = Settings.Localization.Line;
			searchButton.Text = Settings.Localization.Search;

			stationTextBox.Text = station;
		}

		private async void searchButton_Click(object sender, EventArgs e)
		{
			searchButton.Enabled = false;
			var window = await Request.GetStationInfoWindowAsync(stationTextBox.Text, departureDateTimePicker.Value, (int)countNumericUpDown.Value, true, lineComboBox.Text, this);
			searchButton.Enabled = true;

			if (window != null)
			{
				window.Show(DockPanel, DockState);
				Hide();
			}
		}

		private void lineComboBox_DropDown(object sender, EventArgs e)
		{
			lineComboBox.Items.Clear();
			var station = DataFeedDesktop.Basic.Stations.FindByName(stationTextBox.Text);
			if (station != null)
				lineComboBox.Items.AddRange(station.GetThroughgoingRoutes().Select(r => r.Label).Distinct().ToArray());
		}

		private void NewDepartureBoardWindow_Load(object sender, EventArgs e)
		{
			Request.AutoCompleteTextBox(stationTextBox, (string[])DataFeedDesktop.Basic.Stations);
		}

		private void stationTextBox_TextChanged(object sender, EventArgs e)
		{
			lineComboBox.Items.Clear();
			lineComboBox.Text = string.Empty;
			lineComboBox.Enabled = DataFeedDesktop.Basic.Stations.FindByName(stationTextBox.Text) != null;
		}

		private void locationPictureBox_Click(object sender, EventArgs e)
		{
			var station = DataFeedDesktop.Basic.Stations.FindClosestStation(AsyncHelpers.RunSync<Position>(DataFeedDesktop.GeoWatcher.GetCurrentPosition));
			if (station != null)
			{
				stationTextBox.Text = station.Name;
			}
		}
	}
}