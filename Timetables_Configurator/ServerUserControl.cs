using System;
using System.IO;
using System.Windows.Forms;
using System.Xml;
using System.Xml.Serialization;

namespace Timetables.Configurator
{
	/// <summary>
	/// Represents one feed item in the listbox.
	/// </summary>
	[Serializable]
	public class FeedItem
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

					foreach (XmlNode feed in settings.GetElementsByTagName("DataFeed"))
						DataSources.Add(new FeedItem { Name = feed.FirstChild.InnerText, Link = feed.LastChild.InnerText });
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
					"CoefficientUndergroundToSurfaceTransfer", "MaximalDurationOfTransfer", "AverageWalkingSpeed", "DataFeeds");
				
				settings.GetElementsByTagName("RouterPort")[0].InnerText = routerPortTextBox.Text;
				
				settings.GetElementsByTagName("DepartureBoardPort")[0].InnerText = dbPortTextBox.Text;

				settings.GetElementsByTagName("CoefficientUndergroundTransfersWithinSameLine")[0].InnerText = cutwslTextBox.Text;

				settings.GetElementsByTagName("CoefficientUndergroundTransfersWithinDifferentLines")[0].InnerText = cutwdlTextBox.Text;

				settings.GetElementsByTagName("CoefficientUndergroundToSurfaceTransfer")[0].InnerText = cutstTextBox.Text;

				settings.GetElementsByTagName("MaximalDurationOfTransfer")[0].InnerText = maxDurTextBox.Text;

				settings.GetElementsByTagName("AverageWalkingSpeed")[0].InnerText = avgSpeedTextBox.Text;
				
				foreach (var feed in DataSources)
				{
					var name = settings.CreateElement("Name"); name.InnerText = feed.Name;
					var link = settings.CreateElement("Link"); link.InnerText = feed.Link;

					var dataFeed = settings.CreateElement("DataFeed");
					dataFeed.AppendChild(name);
					dataFeed.AppendChild(link);

					settings.GetElementsByTagName("DataFeeds")[0].AppendChild(dataFeed);
				}

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
