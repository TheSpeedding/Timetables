﻿using System;
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

					serverIpTextBox.Text = settings.GetElementsByTagName("ServerIp")[0].InnerText;
					portDepBoardTextBox.Text = settings.GetElementsByTagName("DepartureBoardPort")[0].InnerText;
					portRouterTextBox.Text = settings.GetElementsByTagName("RouterPort")[0].InnerText;
					basicDataTextBox.Text = settings.GetElementsByTagName("BasicDataPort")[0].InnerText;

					bool offline;

					try
					{
						offline = bool.Parse(settings.GetElementsByTagName("OfflineMode")[0].InnerText);
					}
					catch // Mobile app.
					{
						offline = false;
					}

					offlineRadioButton.Checked = offline;
					onlineRadioButton.Checked = !offline;

					if (offline)
						fullDataTextBox.Text = settings.GetElementsByTagName("FullDataUri")[0].InnerText;
				}
				catch
				{
					MessageBox.Show("Settings file is corrupted.", "Unable to load settings file.", MessageBoxButtons.OK, MessageBoxIcon.Error);
				}
			}
		}

		private void offlineRadioButton_CheckedChanged(object sender, EventArgs e)
		{
			basicDataTextBox.Enabled = !offlineRadioButton.Checked;
			fullDataTextBox.Enabled = offlineRadioButton.Checked;
			serverIpTextBox.Enabled = !offlineRadioButton.Checked;
			portRouterTextBox.Enabled = !offlineRadioButton.Checked;
			portDepBoardTextBox.Enabled = !offlineRadioButton.Checked;
		}

		private void saveButton_Click(object sender, EventArgs e)
		{
			if (settingsSaveFileDialog.ShowDialog() == DialogResult.OK)
			{
				using (var sw = new System.IO.StreamWriter(settingsSaveFileDialog.FileName))
					sw.Write("<Timetables></Timetables>");

				XmlDocument settings = new XmlDocument();
				settings.Load(settingsSaveFileDialog.FileName);

				void CreateElementIfNotExist(params string[] names)
				{
					foreach (var name in names)
						if (settings.GetElementsByTagName(name).Count == 0)
							settings.DocumentElement.AppendChild(settings.CreateElement(name));
				}

				CreateElementIfNotExist("Theme", "Language", "OfflineMode", "ExtraEventsUri", "LockoutsUri", "FullDataUri", "ServerIp", "RouterPort", "DepartureBoardPort", "BasicDataPort");

				settings.GetElementsByTagName("Theme")[0].InnerText = "0";

				settings.GetElementsByTagName("Language")[0].InnerText = languageTextBox.Text;

				settings.GetElementsByTagName("OfflineMode")[0].InnerText = offlineRadioButton.Checked.ToString();

				settings.GetElementsByTagName("ExtraEventsUri")[0].InnerText = extraEventsTextBox.Text;

				settings.GetElementsByTagName("LockoutsUri")[0].InnerText = lockoutsTextBox.Text;
				
				settings.GetElementsByTagName("FullDataUri")[0].InnerText = fullDataTextBox.Text;

				settings.GetElementsByTagName("ServerIp")[0].InnerText = serverIpTextBox.Text;

				settings.GetElementsByTagName("RouterPort")[0].InnerText = portRouterTextBox.Text;

				settings.GetElementsByTagName("DepartureBoardPort")[0].InnerText = portDepBoardTextBox.Text;

				settings.GetElementsByTagName("BasicDataPort")[0].InnerText = basicDataTextBox.Text;

				settings.Save(settingsSaveFileDialog.FileName);

				MessageBox.Show("Settings file saved sucessfully.", "Saved sucessfully.", MessageBoxButtons.OK, MessageBoxIcon.Information);
			}
		}

		private void clearButton_Click(object sender, EventArgs e)
		{
			languageTextBox.Text = "English";
			serverIpTextBox.Text = string.Empty;
			portRouterTextBox.Text = string.Empty;
			portDepBoardTextBox.Text = string.Empty;
			basicDataTextBox.Text = string.Empty;
			fullDataTextBox.Text = string.Empty;
			lockoutsTextBox.Text = string.Empty;
			extraEventsTextBox.Text = string.Empty;
		}
	}
}
