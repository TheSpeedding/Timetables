using System;
using System.Windows.Forms;
using WeifenLuo.WinFormsUI.Docking;
using Timetables.Client;

namespace Timetables.Application.Desktop
{
	public partial class NewDepartureBoardWindow : DockContent
	{
		public NewDepartureBoardWindow()
		{
			InitializeComponent();
			Settings.Theme.Apply(this);
			
			foreach (var station in DataFeedGlobals.Basic.Stations)
				stationComboBox.Items.Add(station);	
		}

		private void searchButton_Click(object sender, EventArgs e)
		{
			Structures.Basic.StationsBasic.StationBasic station = null;

			if (stationComboBox.SelectedItem == null)
			{
				station = DataFeedGlobals.Basic.Stations.FindByName(stationComboBox.Text);

				if (station == null)
				{
					MessageBox.Show($"Station with name \"{ stationComboBox.Text }\" was not found.", "Station not found.", MessageBoxButtons.OK, MessageBoxIcon.Warning);
					return;
				}
			}

			else
				station = stationComboBox.SelectedItem as Structures.Basic.StationsBasic.StationBasic;
			
			var dbRequest = new DepartureBoardRequest(station.ID, departureDateTimePicker.Value, (uint)countNumericUpDown.Value, true);
			DepartureBoardResponse dbResponse = null;

			using (var dbProcessing = new Interop.DepartureBoardManaged(DataFeedGlobals.Full, dbRequest))
			{
				dbProcessing.ObtainDepartureBoard();
				dbResponse = dbProcessing.ShowDepartureBoard();
			}
			
			new DepartureBoardResultsWindow(dbResponse, station.Name, departureDateTimePicker.Value).Show(DockPanel, DockState);
			Close();
		}
		
	}
}