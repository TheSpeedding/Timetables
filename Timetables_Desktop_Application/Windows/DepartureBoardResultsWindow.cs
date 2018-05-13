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
	public partial class DepartureBoardResultsWindow : DockContent
	{
		public DepartureBoardResultsWindow(DepartureBoardResponse dbReponse, string stationName, DateTime dateTime)
		{
			InitializeComponent();
			Settings.Theme.Apply(this);

			Text = $"Departures ({ dbReponse.Departures.Count }) - { stationName } - { dateTime.ToShortTimeString() } { dateTime.ToShortDateString() }";
			
			int yPoint = 0;

			foreach (var departure in dbReponse.Departures)
			{
				Control depControl = new DepartureBoardResultControl(departure)
				{
					Location = new Point(0, yPoint),
					Width = resultsPanel.Width,
					Anchor = AnchorStyles.Top | AnchorStyles.Left // | AnchorStyles.Right
				};

				resultsPanel.Controls.Add(depControl);

				yPoint += depControl.Height + 10;
			}
		}
	}
}
