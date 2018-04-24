using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using WeifenLuo.WinFormsUI.Docking;
using Timetables.Structures.Basic;
using Timetables.Client;

namespace Timetables.Application.Desktop
{
	public partial class NewDepartureBoardWindow : DockContent
	{
		public NewDepartureBoardWindow()
		{
			InitializeComponent();
			Settings.Theme.Apply(this);

			var stationList = (from StationsBasic.StationBasic station in DataFeedGlobals.Basic.Stations where station.Name.Contains(textBox1.Text) select station.Name).ToArray();

			textBox1.AutoCompleteCustomSource.AddRange(stationList);
			textBox1.AutoCompleteSource = AutoCompleteSource.CustomSource;
			textBox1.AutoCompleteMode = AutoCompleteMode.SuggestAppend;
		}
	}
}
