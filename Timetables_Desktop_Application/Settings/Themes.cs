using System.Drawing;
using System.Windows.Forms;
using WeifenLuo.WinFormsUI.Docking;

namespace Timetables.Application.Desktop.Themes
{
	/// <summary>
	/// Extension methods for themes.
	/// </summary>
	public static class ThemeExtensions
	{
		/// <summary>
		/// Applies selected theme to the dockpanel.
		/// </summary>
		public static void Apply(this ThemeBase panelTheme, DockPanel panel) => panel.Theme = panelTheme;
	}
	/// <summary>
	/// Class defining theme of the application.
	/// </summary>
	abstract class Theme
	{
		/// <summary>
		/// Theme used in a panel.
		/// </summary>
		public ThemeBase PanelTheme { get; protected set; }
		/// <summary>
		/// Theme used in a menu strip.
		/// </summary>
		public MenuTheme MenuTheme { get; protected set; }
		/// <summary>
		/// Common background color.
		/// </summary>
		public Color BackColor => MenuTheme.Palette.DefaultBackColor;
		/// <summary>
		/// Common text color.
		/// </summary>
		public Color ForeColor => MenuTheme.Palette.TextColor;
		/// <summary>
		/// Applies the theme on the form.
		/// </summary>
		/// <param name="form">Form.</param>
		public void Apply(Form form)
		{
			form.BackColor = BackColor;
			form.ForeColor = ForeColor;
		}
	}

	/// <summary>
	/// Dark theme for the application.
	/// </summary>
	class DarkTheme : Theme
	{
		public DarkTheme()
		{
			PanelTheme = new VS2015DarkTheme();
			MenuTheme = new MenuTheme.DarkTheme();
		}
	}

	/// <summary>
	/// Blue theme for the application.
	/// </summary>
	class BlueTheme : Theme
	{
		public BlueTheme()
		{
			PanelTheme = new VS2015BlueTheme();
			MenuTheme = new MenuTheme.BlueTheme();
		}
	}

	/// <summary>
	/// Light theme for the application.
	/// </summary>
	class LightTheme : Theme
	{
		public LightTheme()
		{
			PanelTheme = new VS2015LightTheme();
			MenuTheme = new MenuTheme.LightTheme();
		}
	}

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
	abstract class MenuTheme : ToolStripProfessionalRenderer
	{
		/// <summary>
		/// Color palette used in the menu strip.
		/// </summary>
		public MenuColors Palette { get; protected set; }
		/// <summary>
		/// Rendered used in the menu.
		/// </summary>
		public ToolStripProfessionalRenderer Renderer { get; protected set; }
		/// <summary>
		/// Applies the theme on the menu strip.
		/// </summary>
		/// <param name="menuStrip">Menu strip.</param>
		public abstract void Apply(MenuStrip menuStrip);
		protected void Apply<T>(MenuStrip menuStrip) where T : MenuColors, new()
		{
			menuStrip.BackColor = Palette.DefaultBackColor;
			menuStrip.ForeColor = Palette.TextColor;

			foreach (var y in menuStrip.Items)
				foreach (var x in (y as ToolStripMenuItem).DropDownItems)
					if (x is ToolStripDropDownItem)
						((ToolStripDropDownItem)x).ForeColor = Palette.TextColor;

			menuStrip.Renderer = Renderer;
		}
		/// <summary>
		/// Dark menu theme.
		/// </summary>
		public class DarkTheme : MenuTheme
		{
			public DarkTheme()
			{
				Palette = new MenuColors.DarkTheme();
				new ToolStripProfessionalRenderer(Palette);
			}
			public override void Apply(MenuStrip menuStrip) => Apply<MenuColors.DarkTheme>(menuStrip);
		}
		/// <summary>
		/// Blue menu theme.
		/// </summary>
		public class BlueTheme : MenuTheme
		{
			public BlueTheme()
			{
				Palette = new MenuColors.BlueTheme();
				new ToolStripProfessionalRenderer(Palette);
			}
			public override void Apply(MenuStrip menuStrip) => Apply<MenuColors.BlueTheme>(menuStrip);
		}
		/// <summary>
		/// Light menu theme.
		/// </summary>
		public class LightTheme : MenuTheme
		{
			public LightTheme()
			{
				Palette = new MenuColors.LightTheme();
				new ToolStripProfessionalRenderer(Palette);
			}
			public override void Apply(MenuStrip menuStrip) => Apply<MenuColors.LightTheme>(menuStrip);
		}
	}
}