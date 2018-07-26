using System;
using System.Windows.Forms;

namespace Timetables.Configurator
{
	public partial class MainWindow : Form
	{
		public MainWindow() => InitializeComponent();		
		private void radioButton_CheckedChanged(object sender, EventArgs e)
		{
			clientUserControl.Visible = clientRadioButton.Checked;
			serverUserControl.Visible = !clientRadioButton.Checked;
		}
	}
}
