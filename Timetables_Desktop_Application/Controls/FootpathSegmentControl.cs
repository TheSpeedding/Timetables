using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
		}
	}
}
