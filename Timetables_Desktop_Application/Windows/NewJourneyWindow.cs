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

			foreach (var station in DataFeedGlobals.Basic.Stations)
			{
				sourceComboBox.Items.Add(station);
				targetComboBox.Items.Add(station);
			}
		}

		private Structures.Basic.StationsBasic.StationBasic GetStationFromComboBox(ComboBox comboBox)
		{

			Structures.Basic.StationsBasic.StationBasic source = null;

			if (comboBox.SelectedItem == null)
			{
				source = DataFeedGlobals.Basic.Stations.FindByName(comboBox.Text);

				if (source == null)
				{
					MessageBox.Show($"Station with name \"{ comboBox.Text }\" was not found.", "Station not found.", MessageBoxButtons.OK, MessageBoxIcon.Warning);
					return null;
				}
			}

			else
				source = comboBox.SelectedItem as Structures.Basic.StationsBasic.StationBasic;

			return source;
		}

		private void searchButton_Click(object sender, EventArgs e)
		{
			Structures.Basic.StationsBasic.StationBasic source = GetStationFromComboBox(sourceComboBox);
			Structures.Basic.StationsBasic.StationBasic target = GetStationFromComboBox(targetComboBox);

			if (source == null || target == null)
				return;


			var jRequest = new RouterRequest(source.ID, target.ID, departureDateTimePicker.Value, (uint)transfersNumericUpDown.Value, (uint)countNumericUpDown.Value, 1);
			RouterResponse jResponse = null;					

			using (var jProcessing = new Interop.RouterManaged(DataFeedGlobals.Full, jRequest))
			{
				jProcessing.ObtainJourneys();
				jResponse = jProcessing.ShowJourneys();
			}

			new JourneyResultsWindow(jResponse, source.Name, target.Name, departureDateTimePicker.Value).Show(DockPanel, DockState);
			Close();
		}

	}
}
