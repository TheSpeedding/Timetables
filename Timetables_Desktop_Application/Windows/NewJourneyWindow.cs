using System;
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

		public NewJourneyWindow(DockPanel panel) : this() => DockPanel = panel;

		private Structures.Basic.StationsBasic.StationBasic GetStationFromString(string name)
		{
			Structures.Basic.StationsBasic.StationBasic source = DataFeed.Basic.Stations.FindByName(name);

			if (source == null)
			{
				MessageBox.Show(Settings.Localization.UnableToFindStation + ": " + name, Settings.Localization.StationNotFound, MessageBoxButtons.OK, MessageBoxIcon.Warning);
				return null;
			}

			return source;
		}

		internal async Task<JourneyResultsWindow> SendRequestAsync(string sourceName, string targetName, DateTime dt, uint transfers, uint count, double coefficient) 
		{
			Structures.Basic.StationsBasic.StationBasic source = GetStationFromString(sourceName);
			Structures.Basic.StationsBasic.StationBasic target = GetStationFromString(targetName);

			if (source == null || target == null)
				return null;

			var routerRequest = new RouterRequest(source.ID, target.ID, dt, transfers, count, coefficient);
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


			return routerResponse == null ? null : new JourneyResultsWindow(routerResponse, source.Name, target.Name, departureDateTimePicker.Value);

		}

		private async void searchButton_Click(object sender, EventArgs e)
		{
			var window = await SendRequestAsync(sourceComboBox.Text, targetComboBox.Text, departureDateTimePicker.Value, (uint)transfersNumericUpDown.Value, (uint)countNumericUpDown.Value, 1);

			if (window != null)
			{
				window.Show(DockPanel, DockState);
				Close();
			}
		}
	}
}
