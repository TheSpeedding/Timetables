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
				sourceComboBox.Items.Add(station);
				targetComboBox.Items.Add(station);
			}
		}

		private Structures.Basic.StationsBasic.StationBasic GetStationFromComboBox(ComboBox comboBox)
		{

			Structures.Basic.StationsBasic.StationBasic source = null;

			if (comboBox.SelectedItem == null)
			{
				source = DataFeed.Basic.Stations.FindByName(comboBox.Text);

				if (source == null)
				{
					MessageBox.Show(Settings.Localization.UnableToFindStation + ": " + comboBox.Text, Settings.Localization.StationNotFound, MessageBoxButtons.OK, MessageBoxIcon.Warning);
					return null;
				}
			}

			else
				source = comboBox.SelectedItem as Structures.Basic.StationsBasic.StationBasic;

			return source;
		}

		private async void searchButton_Click(object sender, EventArgs e)
		{
			Structures.Basic.StationsBasic.StationBasic source = GetStationFromComboBox(sourceComboBox);
			Structures.Basic.StationsBasic.StationBasic target = GetStationFromComboBox(targetComboBox);

			if (source == null || target == null)
				return;

			var jRequest = new RouterRequest(source.ID, target.ID, departureDateTimePicker.Value, (uint)transfersNumericUpDown.Value, (uint)countNumericUpDown.Value, 1);
			RouterResponse jResponse = null;

			if (DataFeed.OfflineMode)
			{
				searchButton.Enabled = false;
				await Task.Run(() =>
				{
					using (var jProcessing = new Interop.RouterManaged(DataFeed.Full, jRequest))
					{
						jProcessing.ObtainJourneys();
						jResponse = jProcessing.ShowJourneys();
					}
				});
				searchButton.Enabled = true;
			}

			else
			{
				searchButton.Enabled = false;
				await Task.Run(async () =>
				{
					using (var jProcessing = new RouterProcessing())
					{
						await jProcessing.ConnectAsync();

						jProcessing.Send(jRequest);

						jResponse = await Task.Run(() => jProcessing.Receive<RouterResponse>());
					}
				});
				searchButton.Enabled = true;
			}


			new JourneyResultsWindow(jResponse, source.Name, target.Name, departureDateTimePicker.Value).Show(DockPanel, DockState);
			Close();
		}

	}
}
