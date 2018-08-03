using System;
using System.Windows.Forms;
using System.Xml;

namespace Timetables.Configurator
{
	/// <summary>
	/// Represents one feed item in the listbox.
	/// </summary>
	[Serializable]
	internal class FeedItem
	{
		/// <summary>
		/// Location name.
		/// </summary>
		public string Name { get; set; }
		/// <summary>
		/// Link to the data feed.
		/// </summary>
		public string Link { get; set; }
		public override string ToString() => Name;
	}
	public partial class ServerUserControl : UserControl
	{
		internal System.Collections.Generic.List<FeedItem> DataSources { get; } = new System.Collections.Generic.List<FeedItem>();

		public ServerUserControl()
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

					routerPortTextBox.Text = settings.GetElementsByTagName("RouterPort")[0].InnerText;
					dbPortTextBox.Text = settings.GetElementsByTagName("DepartureBoardPort")[0].InnerText;
					cutwslTextBox.Text = settings.GetElementsByTagName("CoefficientUndergroundTransfersWithinSameLine")[0].InnerText;
					cutwdlTextBox.Text = settings.GetElementsByTagName("CoefficientUndergroundTransfersWithinDifferentLines")[0].InnerText;
					cutstTextBox.Text = settings.GetElementsByTagName("CoefficientUndergroundToSurfaceTransfer")[0].InnerText;
					maxDurTextBox.Text = settings.GetElementsByTagName("MaximalDurationOfTransfer")[0].InnerText;
					avgSpeedTextBox.Text = settings.GetElementsByTagName("AverageWalkingSpeed")[0].InnerText;					
				}
				catch
				{
					MessageBox.Show("Settings file is corrupted.", "Unable to load settings file.", MessageBoxButtons.OK, MessageBoxIcon.Error);
				}
			}
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

				CreateElementIfNotExist("RouterPort", "DepartureBoardPort", "CoefficientUndergroundTransfersWithinSameLine", "CoefficientUndergroundTransfersWithinDifferentLines",
					"CoefficientUndergroundToSurfaceTransfer", "MaximalDurationOfTransfer", "AverageWalkingSpeed");
				
				settings.GetElementsByTagName("RouterPort")[0].InnerText = routerPortTextBox.Text;
				
				settings.GetElementsByTagName("DepartureBoardPort")[0].InnerText = dbPortTextBox.Text;

				settings.GetElementsByTagName("CoefficientUndergroundTransfersWithinSameLine")[0].InnerText = cutwslTextBox.Text;

				settings.GetElementsByTagName("CoefficientUndergroundTransfersWithinDifferentLines")[0].InnerText = cutwdlTextBox.Text;

				settings.GetElementsByTagName("CoefficientUndergroundToSurfaceTransfer")[0].InnerText = cutstTextBox.Text;

				settings.GetElementsByTagName("MaximalDurationOfTransfer")[0].InnerText = maxDurTextBox.Text;

				settings.GetElementsByTagName("AverageWalkingSpeed")[0].InnerText = avgSpeedTextBox.Text;
				
				settings.Save(settingsSaveFileDialog.FileName);

				MessageBox.Show("Settings file saved sucessfully.", "Saved sucessfully.", MessageBoxButtons.OK, MessageBoxIcon.Information);
			}
		}

		private void clearButton_Click(object sender, EventArgs e)
		{
			routerPortTextBox.Text = string.Empty;
			maxDurTextBox.Text = string.Empty;
			avgSpeedTextBox.Text = string.Empty;
			cutwdlTextBox.Text = string.Empty;
			cutstTextBox.Text = string.Empty;
			cutwslTextBox.Text = string.Empty;
			dbPortTextBox.Text = string.Empty;
		}

		private void sourcesButton_Click(object sender, EventArgs e) => new DataSourcesWindow(this).ShowDialog();
	}
}
