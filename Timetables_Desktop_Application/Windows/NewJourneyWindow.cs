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
			walkingSpeedLabel.Text = Settings.Localization.WalkingSpeed;
			searchButton.Text = Settings.Localization.Search;
			slowButton.Text = Settings.Localization.SlowSpeed;
			mediumButton.Text = Settings.Localization.MediumSpeed;
			fastButton.Text = Settings.Localization.FastSpeed;

			foreach (var station in DataFeed.Basic.Stations)
			{
				sourceComboBox.Items.Add(station.Name);
				targetComboBox.Items.Add(station.Name);
			}
		}
				
		private async void searchButton_Click(object sender, EventArgs e)
		{
			searchButton.Enabled = false;
			var window = await Requests.SendRouterRequestAsync(sourceComboBox.Text, targetComboBox.Text, departureDateTimePicker.Value, (uint)transfersNumericUpDown.Value, (uint)countNumericUpDown.Value, GetTransferCoefficient());
			searchButton.Enabled = true;

			if (window != null)
			{
				window.Show(DockPanel, DockState);
				Close();
			}
		}

		private double GetTransferCoefficient()
		{
			if (slowButton.Checked)
				return 0.5;

			if (mediumButton.Checked)
				return 1;

			if (fastButton.Checked)
				return 1.5;

			throw new InvalidOperationException();
		}
	}
}
