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
	public partial class DepartureBoardResultControl : UserControl
	{
		public DepartureBoardResultControl(Departure departure)
		{
			InitializeComponent();

			outdatedLabel.Enabled = departure.Outdated;

			lineColorPictureBox.BackColor = departure.LineColor;

			meanOfTransportPictureBox.BackColor = departure.LineColor;

			lineDescriptionLabel.BackColor = departure.LineColor;

			// Icons downloaded from here: https://icons8.com/icon/pack/Transport/windows

			switch (departure.MeanOfTransport)
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
			
			lineDescriptionLabel.Text = $"{ departure.LineLabel }  -  { departure.Headsign }";

			departureLabel.Text = $"{ departure.DepartureDateTime.ToShortTimeString() } • { DataFeedGlobals.Basic.Stops.FindByIndex((int)departure.StopID).Name }";

			StringBuilder intStops = new StringBuilder();
			
			foreach (var stop in departure.IntermediateStops)
			{
				intStops.AppendLine($"{ stop.Key.ToShortTimeString() } • { DataFeedGlobals.Basic.Stops.FindByIndex((int)stop.Value).Name }");
			}

			intStopsLabel.Text = intStops.ToString();			
		}

		protected override void OnPaint(PaintEventArgs e)
		{
			base.OnPaint(e);
			ControlPaint.DrawBorder(e.Graphics, ClientRectangle,
								  Color.Black, 1, ButtonBorderStyle.Inset,
								  Color.Black, 1, ButtonBorderStyle.Inset,
								  Color.Black, 1, ButtonBorderStyle.Inset,
								  Color.Black, 1, ButtonBorderStyle.Inset);
		}
	}
}