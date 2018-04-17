using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WeifenLuo.WinFormsUI.Docking;
using System.Windows.Forms;

namespace Timetables.Application.Desktop
{
	public partial class MainWindow : Form
	{
		public MainWindow()
		{
			InitializeComponent();
			mainDockPanel.Theme = Settings.Theme;
		}

		private void settingsToolStripMenuItem_Click(object sender, EventArgs e) => new SettingsWindow().ShowDialog();

		private void findDeparturesToolStripMenuItem_Click(object sender, EventArgs e) => new DepartureBoardWindow().Show(mainDockPanel, DockState.Document);
	}
}
