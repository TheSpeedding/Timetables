using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Timetables.Application.Desktop
{
	/// <summary>
	/// Class defining colors for menu strip.
	/// </summary>
	abstract class MenuColors : ProfessionalColorTable
	{
		public Color DefaultBackColor { get; protected set; }
		public Color TextColor { get; protected set; }
		public Color OnHoverColor { get; protected set; }
		public Color ItemSelectedColor { get; protected set; }
		public Color ItemOnHoverColor { get; protected set; }


		public override Color MenuItemSelected => OnHoverColor;
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

		/// <summary>
		/// Dark theme colors for menu.
		/// </summary>
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

		/// <summary>
		/// Blue theme colors for menu.
		/// </summary>
		public class BlueTheme : MenuColors
		{
			public BlueTheme()
			{
				DefaultBackColor = Color.FromArgb(214, 219, 233);
				TextColor = Color.Black;
				OnHoverColor = Color.FromArgb(253, 244, 191);
				ItemSelectedColor = Color.FromArgb(234, 240, 255);
				ItemOnHoverColor = Color.FromArgb(253, 244, 191);
			}
		}

		/// <summary>
		/// Light theme colors for menu.
		/// </summary>
		public class LightTheme : MenuColors
		{
			public LightTheme()
			{
				DefaultBackColor = Color.FromArgb(238, 238, 242);
				TextColor = Color.Black;
				OnHoverColor = Color.FromArgb(201, 222, 245);
				ItemSelectedColor = Color.FromArgb(246, 246, 246);
				ItemOnHoverColor = Color.FromArgb(201, 222, 245);
			}
		}
	}

	/// <summary>
	/// Class defining menu theme.
	/// </summary>
	/// <typeparam name="T">Color palette used in menu.</typeparam>
	class MenuTheme<T> : ToolStripProfessionalRenderer where T : MenuColors, new()
	{
		private static readonly MenuColors palette = new T();
		public MenuTheme(MenuStrip menuStrip) : base(palette)
		{
			menuStrip.BackColor = palette.DefaultBackColor;
			menuStrip.ForeColor = palette.TextColor;

			foreach (var y in menuStrip.Items)
				foreach (var x in (y as ToolStripMenuItem).DropDownItems)
					if (x is ToolStripDropDownItem)
						((ToolStripDropDownItem)x).ForeColor = palette.TextColor;
		}
	}
}
