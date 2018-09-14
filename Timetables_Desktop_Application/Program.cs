using System;
using System.Net.Mail;
using System.Security.Permissions;
using System.Windows.Forms;
using System.Linq;

namespace Timetables.Application.Desktop
{
	static class Program
	{
		private static MailAddress UnhandledExceptionMailSendTo { get; } = new MailAddress("thespeedding@gmail.com");
		/// <summary>
		/// The main entry point for the application.
		/// </summary>
		[STAThread]
		[SecurityPermission(SecurityAction.Demand, Flags = SecurityPermissionFlag.ControlAppDomain)]
		static void Main()
		{
			System.Threading.Thread.CurrentThread.CurrentCulture = System.Globalization.CultureInfo.InvariantCulture;

			AppDomain.CurrentDomain.UnhandledException += ShowUnhandledExceptionCallback;
			AppDomain.CurrentDomain.UnhandledException += LogUnhandledExceptionCallback;
			AppDomain.CurrentDomain.UnhandledException += (sender, e) => Environment.Exit(0); // End application immediately and don't wait for exception to be arisen from .NET environment.
			
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

				SmtpClient client = new SmtpClient
				{
					Port = 25,
					DeliveryMethod = SmtpDeliveryMethod.Network,
					UseDefaultCredentials = false,
					Host = "smtp.gmail.com"
				};

				MailMessage mail = new MailMessage(UnhandledExceptionMailSendTo, UnhandledExceptionMailSendTo)
				{
					Subject = "Timetables Desktop Application - Unhandled exception",
					Body = messageText
				};

				client.Send(mail);				
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
