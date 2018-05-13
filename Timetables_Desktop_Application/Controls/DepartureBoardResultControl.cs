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

			lineColorPictureBox.BackColor = departure.LineColor;

			meanOfTransportPictureBox.BackColor = departure.LineColor;

			lineDescriptionLabel.BackColor = departure.LineColor;

			switch (departure.MeanOfTransport)
			{
				case MeanOfTransport.Tram:
					break;
				case MeanOfTransport.Subway:
					break;
				case MeanOfTransport.Rail:
					break;
				case MeanOfTransport.Bus:
					break;
				case MeanOfTransport.Ship:
					break;
				case MeanOfTransport.CableCar:
					break;
				case MeanOfTransport.Gondola:
					break;
				case MeanOfTransport.Funicular:
					break;
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