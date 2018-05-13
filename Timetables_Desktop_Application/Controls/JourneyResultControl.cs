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
	public partial class JourneyResultControl : UserControl
	{
		public JourneyResultControl(Journey journey)
		{
			InitializeComponent();

			AutoSize = true;

			int yPoint = 1;

			foreach (var seg in journey.JourneySegments)
			{
				Control segControl = null;

				if (seg is TripSegment)
				{
					segControl = new TripSegmentControl(seg as TripSegment)
					{
						Location = new Point(1, yPoint),
						Width = Width - 2,
						Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right
					};
				}

				else if (seg is FootpathSegment)
				{
					segControl = new FootpathSegmentControl(seg as FootpathSegment)
					{
						Location = new Point(1, yPoint),
						Width = Width - 2,
						Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right
					};
				}

				Controls.Add(segControl);

				yPoint += segControl.Height + 10;
			}
		}

		protected override void OnPaint(PaintEventArgs e)
		{
			base.OnPaint(e);
			ControlPaint.DrawBorder(e.Graphics, ClientRectangle,
								  Color.Black, 1, ButtonBorderStyle.Inset,
								  Color.Black, 1, ButtonBorderStyle.Inset,
								  Color.Black, 1, ButtonBorderStyle.Inset,
								  Color.Black, 1, ButtonBorderStyle.Inset);
		}
	}
}
