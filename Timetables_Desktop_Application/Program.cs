using System;
using System.Security.Permissions;
using System.Windows.Forms;

namespace Timetables.Application.Desktop
{
	static class Program
	{
		/// <summary>
		/// The main entry point for the application.
		/// </summary>
		[STAThread]
		[SecurityPermission(SecurityAction.Demand, Flags = SecurityPermissionFlag.ControlAppDomain)]
		static void Main()
		{
			AppDomain.CurrentDomain.UnhandledException += UnhandledExceptionCallback;

			System.Windows.Forms.Application.EnableVisualStyles();
			System.Windows.Forms.Application.SetCompatibleTextRenderingDefault(false);

			Settings.Load();

			var initWindow = new InitLoadingWindow();
			System.Windows.Forms.Application.Run(initWindow);

			if (!initWindow.IsFaulted) // Continue executing only if there was no error while initializing.
				System.Windows.Forms.Application.Run(new MainWindow());
		}

		/// <summary>
		/// Callback to handle unhandled exception.
		/// </summary>
		internal static void UnhandledExceptionCallback(object sender, UnhandledExceptionEventArgs e) => MessageBox.Show(((Exception)e.ExceptionObject).Message, ((Exception)e.ExceptionObject).Message, MessageBoxButtons.OK, MessageBoxIcon.Error);
	}
}
