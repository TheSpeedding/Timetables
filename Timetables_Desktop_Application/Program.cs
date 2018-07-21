using System;

namespace Timetables.Application.Desktop
{
	static class Program
	{
		/// <summary>
		/// The main entry point for the application.
		/// </summary>
		[STAThread]
		static void Main()
		{
			System.Windows.Forms.Application.EnableVisualStyles();
			System.Windows.Forms.Application.SetCompatibleTextRenderingDefault(false);
			Settings.Load();
			System.Windows.Forms.Application.Run(new InitLoadingWindow());
			System.Windows.Forms.Application.Run(new MainWindow());
		}
	}
}
