using System;
using System.Windows.Forms;
using WeifenLuo.WinFormsUI.Docking;
using Timetables.Client;
using System.Threading.Tasks;

namespace Timetables.Application.Desktop
{
	public partial class NewDepartureBoardWindow : DockContent
	{
		public NewDepartureBoardWindow()
		{
			InitializeComponent();
			Settings.Theme.Apply(this);

			Text = Settings.Localization.NewDepartureBoard;

			countLabel.Text = Settings.Localization.Count;
			departureLabel.Text = Settings.Localization.LeavingTime;
			stationLabel.Text = Settings.Localization.Station;

			foreach (var station in DataFeed.Basic.Stations)
				stationComboBox.Items.Add(station);	
		}

		private async void searchButton_Click(object sender, EventArgs e)
		{
			Structures.Basic.StationsBasic.StationBasic station = null;

			if (stationComboBox.SelectedItem == null)
			{
				station = DataFeed.Basic.Stations.FindByName(stationComboBox.Text);

				if (station == null)
				{
					MessageBox.Show(Settings.Localization.UnableToFindStation + ": " + stationComboBox.Text, Settings.Localization.StationNotFound, MessageBoxButtons.OK, MessageBoxIcon.Warning);
					return;
				}
			}

			else
				station = stationComboBox.SelectedItem as Structures.Basic.StationsBasic.StationBasic;
			
			var dbRequest = new DepartureBoardRequest(station.ID, departureDateTimePicker.Value, (uint)countNumericUpDown.Value, true);
			DepartureBoardResponse dbResponse = null;

			if (DataFeed.OfflineMode)
			{
				searchButton.Enabled = false;
				await Task.Run(() =>
				{
					using (var dbProcessing = new Interop.DepartureBoardManaged(DataFeed.Full, dbRequest))
					{
						dbProcessing.ObtainDepartureBoard();
						dbResponse = dbProcessing.ShowDepartureBoard();
					}
				});
				searchButton.Enabled = true;
			}
			
			else
			{
				searchButton.Enabled = false;
				await Task.Run(async () =>
				{
					using (var dbProcessing = new DepartureBoardProcessing())
					{
						var connection = dbProcessing.ConnectAsync();

						if (await Task.WhenAny(connection, Task.Delay(Settings.TimeoutDuration)) == connection)
						{
							dbProcessing.Send(dbRequest);

							dbResponse = await Task.Run(() => dbProcessing.Receive<DepartureBoardResponse>());
						}

						else
							MessageBox.Show(Settings.Localization.UnreachableHost, Settings.Localization.Offline, MessageBoxButtons.OK, MessageBoxIcon.Error);
					}					
				});
				searchButton.Enabled = true;
			}

			if (dbResponse != null)
			{
				new DepartureBoardResultsWindow(dbResponse, station.Name, departureDateTimePicker.Value).Show(DockPanel, DockState);

				Close();
			}
		}		
	}
}