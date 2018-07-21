using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml;

namespace Timetables.Configurator
{
	public partial class ClientUserControl : UserControl
	{
		public ClientUserControl()
		{
			InitializeComponent();
		}

		private void loadFileButton_Click(object sender, EventArgs e)
		{
			if (settingsOpenFileDialog.ShowDialog() == DialogResult.OK)
			{
				try
				{
					XmlDocument settings = new XmlDocument();
					settings.Load(settingsOpenFileDialog.FileName);

					if (settings.GetElementsByTagName("Language")[0] != null)
						languageTextBox.Text = settings.GetElementsByTagName("Language")[0].InnerText;

					extraEventsTextBox.Text = settings.GetElementsByTagName("ExtraEventsUri")[0].InnerText;
					lockoutsTextBox.Text = settings.GetElementsByTagName("LockoutsUri")[0].InnerText;
					basicDataTextBox.Text = settings.GetElementsByTagName("BasicDataUri")[0].InnerText;
					fullDataTextBox.Text = settings.GetElementsByTagName("FullDataUri")[0].InnerText;

					bool offline = bool.Parse(settings.GetElementsByTagName("OfflineMode")[0].InnerText);
					offlineRadioButton.Checked = offline;
					onlineRadioButton.Checked = !offline;
				}
				catch
				{
					MessageBox.Show("Settings file is corrupted.", "Unable to load settings file.", MessageBoxButtons.OK, MessageBoxIcon.Error);
				}
			}
		}

		private void offlineRadioButton_CheckedChanged(object sender, EventArgs e) => fullDataTextBox.Enabled = offlineRadioButton.Checked;

		private void onlineRadioButton_CheckedChanged(object sender, EventArgs e) => basicDataTextBox.Enabled = onlineRadioButton.Checked;
	}
}
