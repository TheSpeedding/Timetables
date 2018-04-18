using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Timetables.Application.Desktop
{
	abstract class MenuColors : ProfessionalColorTable
	{
		public Color DefaultBackColor { get; protected set; }
		public Color TextColor { get; protected set; }
		public Color OnHoverColor { get; protected set; }
		public Color ItemSelectedColor { get; protected set; }
		public Color ItemOnHoverColor { get; protected set; }


		public override Color MenuItemSelected => ItemSelectedColor;
		public override Color MenuItemSelectedGradientBegin => OnHoverColor;
		public override Color MenuItemSelectedGradientEnd => OnHoverColor;
		public override Color MenuItemBorder => OnHoverColor;
		public override Color MenuItemPressedGradientBegin => ItemSelectedColor;
		public override Color MenuItemPressedGradientEnd => ItemSelectedColor;
		public override Color MenuBorder => OnHoverColor;
		public override Color ToolStripBorder => OnHoverColor;
		public override Color ImageMarginGradientBegin => ItemSelectedColor;
		public override Color ImageMarginGradientEnd => ItemSelectedColor;
		public override Color ToolStripDropDownBackground => ItemSelectedColor;
		public override Color ImageMarginGradientMiddle => ItemSelectedColor;
		public override Color SeparatorDark => ItemOnHoverColor;
		public override Color SeparatorLight => ItemOnHoverColor;


		public class DarkTheme : MenuColors
		{
			public DarkTheme()
			{
				DefaultBackColor = Color.FromArgb(45, 45, 48);
				TextColor = Color.White;
				OnHoverColor = Color.FromArgb(62, 62, 64);
				ItemSelectedColor = Color.FromArgb(27, 27, 28);
				ItemOnHoverColor = Color.FromArgb(51, 51, 55);
			}
		}
	}
	class ThemeMenu<T> : ToolStripProfessionalRenderer where T : MenuColors, new()
	{
		private MenuColors palette = new MenuColors.DarkTheme();
		public ThemeMenu(MenuStrip menuStrip) : base(new T())
		{
			var palette = new T();
			menuStrip.BackColor = palette.DefaultBackColor;
			menuStrip.ForeColor = palette.TextColor;

			foreach (var y in menuStrip.Items)
				foreach (var x in (y as ToolStripMenuItem).DropDownItems)
					if (x is ToolStripDropDownItem)
						((ToolStripDropDownItem)x).ForeColor = palette.TextColor;
		}
	}
}
