using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Timetables.Client;

namespace Timetables.Server
{
	static class Program
	{
		private static MailAddress UnhandledExceptionMailSendTo { get; } = new MailAddress("thespeedding@gmail.com");
		static void Main()
		{
			if (Logging.FileForLogging != null)
				Logging.LoggingEvent += (string message) => message.LogWithDateTime(Logging.FileForLogging);

			Logging.LoggingEvent += (string message) => message.LogWithDateTime(Console.Out);

			AppDomain.CurrentDomain.UnhandledException += ReportUnhandledExceptionCallback;

			try
			{
				while (!DataFeed.Loaded) ; // Temporary. Actually, this does nothing. Just forces data to be loaded (static class constructor). Without this, the data would be loaded when the first request approaches the server.
			}
			catch (Exception ex)
			{
				Logging.Log("Fatal error. Data could not be processed: " + ex.Message);
				return;
			}

			Server.Start(IPAddress.Any, Settings.RouterPort, Settings.DepartureBoardPort, Settings.BasicDataFeedPort);

			Server.ServerManipulation();

			if (!Server.IsStopped)
				Server.Stop();
		}

		/// <summary>
		/// Callback to send a report about unhandled exception.
		/// </summary>
		internal static void ReportUnhandledExceptionCallback(object sender, UnhandledExceptionEventArgs e)
		{
			string messageText = ((Exception)e.ExceptionObject).Message + Environment.NewLine + ((Exception)e.ExceptionObject).StackTrace;

			try
			{
				using (var sr = new System.IO.StreamReader("settings.xml"))
					messageText += Environment.NewLine + sr.ReadToEnd();
			}
			catch
			{

			}

			try
			{
				SmtpClient client = new SmtpClient
				{
					Port = 587,
					EnableSsl = true,
					Credentials = new System.Net.NetworkCredential("timetablesmffuk", "timetables2018ksi"),
					Host = "smtp.gmail.com"
				};

				MailMessage mail = new MailMessage(UnhandledExceptionMailSendTo, UnhandledExceptionMailSendTo)
				{
					Subject = "Timetables Server Application - Unhandled exception",
					Body = messageText
				};

				client.Send(mail);
			}
			catch
			{

			}
		}
	}
}