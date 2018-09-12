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
			AppDomain.CurrentDomain.UnhandledException += ShowUnhandledExceptionCallback;
			AppDomain.CurrentDomain.UnhandledException += LogUnhandledExceptionCallback;

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
		internal static void ShowUnhandledExceptionCallback(object sender, UnhandledExceptionEventArgs e)
		{
			if (MessageBox.Show(((Exception)e.ExceptionObject).Message + Environment.NewLine + "Do you wish to send a report?", 
				((Exception)e.ExceptionObject).Message, MessageBoxButtons.YesNo, MessageBoxIcon.Error) == DialogResult.Yes)
			{
				string messageText = ((Exception)e.ExceptionObject).Message + Environment.NewLine + ((Exception)e.ExceptionObject).StackTrace;

				try
				{
					using (var sr = new System.IO.StreamReader(".settings"))
						messageText += Environment.NewLine + sr.ReadToEnd();
				}
				catch
				{

				}

				/*
				System.Net.Mail.MailMessage message = new System.Net.Mail.MailMessage();
				message.To.Add("thespeedding@gmail.com");
				message.Subject = "Timetables Desktop Application - Unhandled exception";
				message.From = new System.Net.Mail.MailAddress("exceptions@timetables.cz");
				message.Body = messageText;
				System.Net.Mail.SmtpClient smtp = new System.Net.Mail.SmtpClient("smtp.gmail.com");
				smtp.Send(message);
				*/
			}
		}

		/// <summary>
		/// Callback to handle unhandled exception.
		/// </summary>
		internal static void LogUnhandledExceptionCallback(object sender, UnhandledExceptionEventArgs e)
		{
			using (var sw = new System.IO.StreamWriter("exceptions.txt", true))
			{
				sw.WriteLine(DateTime.Now);
				sw.WriteLine(((Exception)e.ExceptionObject).Message);
				sw.WriteLine(((Exception)e.ExceptionObject).StackTrace);
				try
				{
					using (var sr = new System.IO.StreamReader(".settings"))
						sw.WriteLine(sr.ReadToEnd());
				}
				catch
				{

				}
				sw.WriteLine();
			}
		}
	}
}
