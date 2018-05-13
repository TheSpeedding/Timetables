using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using WeifenLuo.WinFormsUI.Docking;
using Timetables.Client;

namespace Timetables.Application.Desktop
{
	public partial class JourneyResultsWindow : DockContent
	{
		public JourneyResultsWindow(RouterResponse jResponse, string source, string target, DateTime dateTime)
		{
			InitializeComponent();
			Settings.Theme.Apply(this);

			Text = $"Departures ({ jResponse.Journeys.Count }) - { source } - { target } - { dateTime.ToShortTimeString() } { dateTime.ToShortDateString() }";

			int yPoint = 0;

			foreach (var journey in jResponse.Journeys)
			{
				Control jControl = new JourneyResultControl(journey)
				{
					Location = new Point(0, yPoint),
					Width = resultsPanel.Width,
					Anchor = AnchorStyles.Top | AnchorStyles.Left // | AnchorStyles.Right
				};

				resultsPanel.Controls.Add(jControl);

				yPoint += jControl.Height + 10;
			}
		}
	}
}
