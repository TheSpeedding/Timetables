﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Timetables.Client;
using WeifenLuo.WinFormsUI.Docking;

namespace Timetables.Application.Desktop
{
	public partial class NewJourneyWindow : DockContent
	{

		public NewJourneyWindow()
		{
			InitializeComponent();
			Settings.Theme.Apply(this);

			Text = Settings.Localization.NewJourney;

			sourceLabel.Text = Settings.Localization.SourceStop;
			targetLabel.Text = Settings.Localization.TargetStop;
			countLabel.Text = Settings.Localization.Count;
			departureLabel.Text = Settings.Localization.LeavingTime;
			transfersLabel.Text = Settings.Localization.TransfersCount;
			searchButton.Text = Settings.Localization.Search;

			foreach (var station in DataFeed.Basic.Stations)
			{
				sourceComboBox.Items.Add(station.Name);
				targetComboBox.Items.Add(station.Name);
			}
		}

		private Structures.Basic.StationsBasic.StationBasic GetStationFromComboBox(ComboBox comboBox)
		{
			Structures.Basic.StationsBasic.StationBasic source = DataFeed.Basic.Stations.FindByName(comboBox.Text);

			if (source == null)
			{
				MessageBox.Show(Settings.Localization.UnableToFindStation + ": " + comboBox.Text, Settings.Localization.StationNotFound, MessageBoxButtons.OK, MessageBoxIcon.Warning);
				return null;
			}

			return source;
		}

		private async void searchButton_Click(object sender, EventArgs e)
		{
			Structures.Basic.StationsBasic.StationBasic source = GetStationFromComboBox(sourceComboBox);
			Structures.Basic.StationsBasic.StationBasic target = GetStationFromComboBox(targetComboBox);

			if (source == null || target == null)
				return;

			var routerRequest = new RouterRequest(source.ID, target.ID, departureDateTimePicker.Value, (uint)transfersNumericUpDown.Value, (uint)countNumericUpDown.Value, 1);
			RouterResponse routerResponse = null;

			if (DataFeed.OfflineMode)
			{
				searchButton.Enabled = false;
				await Task.Run(() =>
				{
					using (var routerProcessing = new Interop.RouterManaged(DataFeed.Full, routerRequest))
					{
						routerProcessing.ObtainJourneys();
						routerResponse = routerProcessing.ShowJourneys();
					}
				});
				searchButton.Enabled = true;
			}

			else
			{
				searchButton.Enabled = false;
				
				using (var routerProcessing = new RouterProcessing())
				{
					try
					{
						routerResponse = await routerProcessing.ProcessAsync(routerRequest, Settings.TimeoutDuration);
					}
					catch (System.Net.WebException)
					{
						MessageBox.Show(Settings.Localization.UnreachableHost, Settings.Localization.Offline, MessageBoxButtons.OK, MessageBoxIcon.Error);
					}
				}
				
				searchButton.Enabled = true;
			}

			if (routerResponse != null)
			{
				new JourneyResultsWindow(routerResponse, source.Name, target.Name, departureDateTimePicker.Value).Show(DockPanel, DockState);

				Close();
			}
		}

	}
}
