using System;
using System.Windows.Forms;
using WeifenLuo.WinFormsUI.Docking;
using Timetables.Client;
using System.Linq;
using System.Threading.Tasks;

namespace Timetables.Application.Desktop
{
	public partial class NewLineInfoWindow : DockContent
	{
		public NewLineInfoWindow(string station = "")
		{
			InitializeComponent();
			Settings.Theme.Apply(this);

			Text = Settings.Localization.NewLineInfo;

			countLabel.Text = Settings.Localization.Count;
			departureLabel.Text = Settings.Localization.LeavingTime;
			lineLabel.Text = Settings.Localization.Line;
			searchButton.Text = Settings.Localization.Search;

			lineTextBox.Text = station;
		}

		private async void searchButton_Click(object sender, EventArgs e)
		{
			searchButton.Enabled = false;
			var window = await Requests.GetLineInfoWindowAsync(departureDateTimePicker.Value, (int)countNumericUpDown.Value, lineTextBox.Text, this);
			searchButton.Enabled = true;

			if (window != null)
			{
				window.Show(DockPanel, DockState);
				Hide();
			}
		}
		
		private void NewDepartureBoardWindow_Load(object sender, EventArgs e)
		{
			Requests.AutoCompleteTextBox(lineTextBox, (string[])DataFeedDesktop.Basic.RoutesInfo);
		}
	}
}