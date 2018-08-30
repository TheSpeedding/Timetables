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
				stationComboBox.Items.Add(station.Name);	
		}

		private async void searchButton_Click(object sender, EventArgs e)
		{
			searchButton.Enabled = false;
			var window = await Requests.SendDepartureBoardRequestAsync(stationComboBox.Text, departureDateTimePicker.Value, (uint)countNumericUpDown.Value, true);
			searchButton.Enabled = true;

			if (window != null)
			{
				window.Show(DockPanel, DockState);
				Close();
			}
		}		
	}
}