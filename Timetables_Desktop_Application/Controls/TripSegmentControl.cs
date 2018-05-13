using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Timetables.Client;

namespace Timetables.Application.Desktop
{
	public partial class TripSegmentControl : UserControl
	{
		public TripSegmentControl(TripSegment segment)
		{
			InitializeComponent();

			lineColorPictureBox.BackColor = segment.LineColor;

			meanOfTransportPictureBox.BackColor = segment.LineColor;

			lineDescriptionLabel.BackColor = segment.LineColor;

			lineDescriptionTooltip.SetToolTip(lineDescriptionLabel, segment.LineName);

			// Icons downloaded from here: https://icons8.com/icon/pack/Transport/windows

			switch (segment.MeanOfTransport)
			{
				case MeanOfTransport.Tram:
					meanOfTransportPictureBox.Image = Properties.Resources.icons8_tram_100;
					break;
				case MeanOfTransport.Subway:
					meanOfTransportPictureBox.Image = Properties.Resources.icons8_subway_100;
					break;
				case MeanOfTransport.Rail:
					meanOfTransportPictureBox.Image = Properties.Resources.icons8_train_100;
					break;
				case MeanOfTransport.Bus:
					meanOfTransportPictureBox.Image = Properties.Resources.icons8_bus_100;
					break;
				case MeanOfTransport.Ship:
					meanOfTransportPictureBox.Image = Properties.Resources.icons8_water_transportation_100;
					break;
				case MeanOfTransport.CableCar:
					meanOfTransportPictureBox.Image = Properties.Resources.icons8_cable_car_100;
					break;
				case MeanOfTransport.Gondola:
					goto case MeanOfTransport.CableCar;
				case MeanOfTransport.Funicular:
					goto case MeanOfTransport.CableCar;
			}

			lineDescriptionLabel.Text = $"{ segment.LineLabel }  -  { segment.Headsign }";

			departureLabel.Text = $"{ segment.DepartureDateTime.ToShortTimeString() } • { DataFeedGlobals.Basic.Stops.FindByIndex((int)segment.SourceStopID).Name }";

			arrivalLabel.Text = $"{ segment.ArrivalDateTime.ToShortTimeString() } • { DataFeedGlobals.Basic.Stops.FindByIndex((int)segment.TargetStopID).Name }";

			StringBuilder intStops = new StringBuilder();

			foreach (var stop in segment.IntermediateStops)
			{
				intStops.AppendLine($"{ stop.Key.ToShortTimeString() } • { DataFeedGlobals.Basic.Stops.FindByIndex((int)stop.Value).Name }");
			}

			intStopsLabel.Text = intStops.ToString();

			if (segment.IntermediateStops.Count != 0)
				arrivalLabel.Location = new Point(arrivalLabel.Location.X, intStopsLabel.Location.Y + intStopsLabel.Height + 5);

			intStopsLabel.Enabled = segment.IntermediateStops.Count != 0;
		}
	}
}
