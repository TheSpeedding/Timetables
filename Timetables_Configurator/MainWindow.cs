using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Timetables.Configurator
{
	public partial class MainWindow : Form
	{
		public MainWindow() => InitializeComponent();

		private void serverRadioButton_CheckedChanged(object sender, EventArgs e) { }

		private void clientRadioButton_CheckedChanged(object sender, EventArgs e) => clientUserControl.Visible = clientRadioButton.Checked;
	}
}
