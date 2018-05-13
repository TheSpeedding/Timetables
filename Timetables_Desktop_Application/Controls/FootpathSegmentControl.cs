using System.Windows.Forms;
using Timetables.Client;

namespace Timetables.Application.Desktop
{
	public partial class FootpathSegmentControl : UserControl
	{
		public FootpathSegmentControl(FootpathSegment segment)
		{
			InitializeComponent();

			durationLabel.Text += $"{ segment.Duration.Minutes } minutes, { segment.Duration.Seconds } seconds";

			durationLabel.BackColor = lineColorPictureBox.BackColor;

			meanOfTransportPictureBox.BackColor = lineColorPictureBox.BackColor;
		}
	}
}
