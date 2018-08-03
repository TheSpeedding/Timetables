using Newtonsoft.Json.Linq;
using System.Net;
using System.Windows.Forms;

namespace Timetables.Configurator
{
	public partial class DataSourcesWindow : Form
	{
		private ServerUserControl suc;
		public DataSourcesWindow(ServerUserControl suc)
		{
			InitializeComponent();
			this.suc = suc;

			try
			{
				for (int i = 0; i < suc.DataSources.Count; i++)
				{
					dataFeedsListBox.Items.Add(suc.DataSources[i]);
					dataFeedsListBox.SetItemCheckState(i, CheckState.Checked);
				}

				string jsonDoc;
				using (WebClient wc = new WebClient())
				{
					jsonDoc = wc.DownloadString(@"https://api.transitfeeds.com/v1/getFeeds?key=3dde1af5-a212-4734-850a-5b58f46bd0df&location=undefined&descendants=1&page=1&limit=10000&type=gtfs");
				}

				foreach (var feed in (JArray)JObject.Parse(jsonDoc)["results"]["feeds"])
					dataFeedsListBox.Items.Add(new FeedItem { Name = (string)feed["t"], Link = (string)feed["u"]["d"] });
			}
			catch
			{

			}
		}

		private void addButton_Click(object sender, System.EventArgs e)
		{
			dataFeedsListBox.Items.Insert(0, new FeedItem { Name = nameTextBox.Text, Link = linkTextBox.Text });
			dataFeedsListBox.SetItemCheckState(0, CheckState.Checked);
		}

		private void DataSourcesWindow_FormClosed(object sender, FormClosedEventArgs e)
		{
			suc.DataSources.Clear();
			foreach (FeedItem feed in dataFeedsListBox.CheckedItems)
				suc.DataSources.Add(feed);
		}
	}
}
