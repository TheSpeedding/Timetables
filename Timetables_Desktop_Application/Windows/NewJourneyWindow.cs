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
using static Timetables.Client.Journey;

namespace Timetables.Application.Desktop
{
	public partial class NewJourneyWindow : DockContent
	{
		private class CustomJourneyComparer
		{
			private string name;
			public IComparer<Journey> Comparer { get; }
			public CustomJourneyComparer(IComparer<Journey> comp, string name)
			{
				Comparer = comp;
				this.name = name;
			}
			public override string ToString() => name;
		}

		private IComparer<Journey> CurrentComparer => ((CustomJourneyComparer)sortComboBox.SelectedItem).Comparer; 

		public NewJourneyWindow(string source = "", string target = "")
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
			busCheckBox.Text = Settings.Localization.Bus;
			cablecarCheckBox.Text = Settings.Localization.Cablecar;
			shipCheckBox.Text = Settings.Localization.Ship;
			subwayCheckBox.Text = Settings.Localization.Subway;
			trainCheckBox.Text = Settings.Localization.Train;
			tramCheckBox.Text = Settings.Localization.Tram;
			motLabel.Text = Settings.Localization.MeanOfTransport;

			sortLabel.Text = Settings.Localization.Sort;

			var defaultComparer = new CustomJourneyComparer(new ArrivalTimeComparer(), Settings.Localization.ArrivalTimeAscending);

			sortComboBox.Items.Add(defaultComparer);
			sortComboBox.Items.Add(new CustomJourneyComparer(new DepartureTimeComparer(), Settings.Localization.DepartureTimeAscending));
			sortComboBox.Items.Add(new CustomJourneyComparer(new WaitingTimesComparer(), Settings.Localization.WaitingTimesAscending));
			sortComboBox.Items.Add(new CustomJourneyComparer(new TransfersCountComparer(), Settings.Localization.TransfersCountAscending));

			sortComboBox.SelectedItem = defaultComparer;
			sortComboBox.Text = sortComboBox.SelectedItem.ToString();

			sourceTextBox.Text = source;
			targetTextBox.Text = target;
		}
				
		private async void searchButton_Click(object sender, EventArgs e)
		{
			searchButton.Enabled = false;
			var window = await Request.GetRouterWindowAsync(sourceTextBox.Text, targetTextBox.Text, departureDateTimePicker.Value, (int)transfersNumericUpDown.Value, (int)countNumericUpDown.Value, GetTransferCoefficient(), GetMeanOfTransport(), this, CurrentComparer);
			searchButton.Enabled = true;

			if (window != null)
			{
				window.Show(DockPanel, DockState);
				Hide();
			}
		}

		private MeanOfTransport GetMeanOfTransport()
		{
			MeanOfTransport mot = 0;
			if (busCheckBox.Checked) mot |= MeanOfTransport.Bus;
			if (subwayCheckBox.Checked) mot |= MeanOfTransport.Subway;
			if (tramCheckBox.Checked) mot |= MeanOfTransport.Tram;
			if (trainCheckBox.Checked) mot |= MeanOfTransport.Rail;
			if (shipCheckBox.Checked) mot |= MeanOfTransport.Ship;
			if (cablecarCheckBox.Checked) mot |= MeanOfTransport.CableCar | MeanOfTransport.Funicular | MeanOfTransport.Gondola;
			return mot;
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

		private void NewJourneyWindow_Load(object sender, EventArgs e)
		{
			Request.AutoCompleteTextBox(sourceTextBox, (string[])DataFeedDesktop.Basic.Stations);
			Request.AutoCompleteTextBox(targetTextBox, (string[])DataFeedDesktop.Basic.Stations);
		}

		private void locationPictureBox_Click(object sender, EventArgs e)
		{
			var station = DataFeedDesktop.Basic.Stations.FindClosestStation(DataFeedDesktop.GeoWatcher.Position.Location);
			if (station != null)
			{
				sourceTextBox.Text = station.Name;
			}
		}
	}
}
