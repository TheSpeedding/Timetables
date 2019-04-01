using System;

using Android.App;
using Android.Content.PM;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.OS;
using Android;
using Android.Content;
using System.Net.Mail;
using Timetables.Client;

namespace Timetables.Application.Mobile.Droid
{
	[Activity(Label = "Timetables", Icon = "@mipmap/icon", Theme = "@style/MainTheme", MainLauncher = true, ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation)]
	public class MainActivity : global::Xamarin.Forms.Platform.Android.FormsAppCompatActivity
	{
		private static MailAddress UnhandledExceptionMailSendTo { get; } = new MailAddress("thespeedding@gmail.com");
		public override void OnRequestPermissionsResult(int requestCode, string[] permissions, Android.Content.PM.Permission[] grantResults)
		{
			base.OnRequestPermissionsResult(requestCode, permissions, grantResults);

			bool isPermissionForAllGranted = false;
			if (grantResults.Length > 0 && permissions.Length == grantResults.Length)
			{
				for (int i = 0; i < permissions.Length; i++)
				{
					if (grantResults[i] == Permission.Granted)
					{
						isPermissionForAllGranted = true;
					}
					else
					{
						isPermissionForAllGranted = false;
					}
				}
			}

			else
			{
				isPermissionForAllGranted = true;
			}

			if (isPermissionForAllGranted)
			{
				RunApp(savedInstanceState);
			}
		}

		private Bundle savedInstanceState;

		protected override void OnCreate(Bundle savedInstanceState)
		{
			TabLayoutResource = Resource.Layout.Tabbar;
			ToolbarResource = Resource.Layout.Toolbar;

			base.OnCreate(savedInstanceState);

			this.savedInstanceState = savedInstanceState;

			global::Xamarin.Forms.Forms.Init(this, savedInstanceState);

			bool permissionsGranted = false;

			var permissions = new string[] {
						Manifest.Permission.ReadExternalStorage,
						Manifest.Permission.WriteExternalStorage,
						Manifest.Permission.AccessNetworkState,
						Manifest.Permission.AccessCoarseLocation,
						Manifest.Permission.AccessFineLocation,
						Manifest.Permission.Internet,
						Manifest.Permission.AccessWifiState,
						Manifest.Permission.ReceiveBootCompleted
			};

			if ((int)Build.VERSION.SdkInt >= 23)
			{
				if (
					PackageManager.CheckPermission(permissions[0], PackageName) == Permission.Granted &&
					PackageManager.CheckPermission(permissions[1], PackageName) == Permission.Granted &&
					PackageManager.CheckPermission(permissions[2], PackageName) == Permission.Granted &&
					PackageManager.CheckPermission(permissions[3], PackageName) == Permission.Granted &&
					PackageManager.CheckPermission(permissions[4], PackageName) == Permission.Granted &&
					PackageManager.CheckPermission(permissions[5], PackageName) == Permission.Granted &&
					PackageManager.CheckPermission(permissions[6], PackageName) == Permission.Granted &&
					PackageManager.CheckPermission(permissions[7], PackageName) == Permission.Granted)
				{
					permissionsGranted = true;
				}
				else
				{
					RequestPermissions(permissions, 1);
				}
			}

			else
			{
				permissionsGranted = true; // Permissions granted during installation.
			}

			if (permissionsGranted)
				RunApp(savedInstanceState);

		}

		private void RunApp(Bundle savedInstanceState)
		{
			Xamarin.FormsGoogleMaps.Init(this, savedInstanceState);

			PlatformDependentSettings.GetStream = (fileInfo) => Assets.Open(fileInfo.FullName.Substring(1));

			PlatformDependentSettings.GetLocalizations = () => Assets.List("loc");

			PlatformDependentSettings.BasePath = Android.OS.Environment.ExternalStorageDirectory.AbsolutePath + "/timetables/";

			PlatformDependentSettings.ShowMessage = (message) => Toast.MakeText(this, message, ToastLength.Long).Show();

			PlatformDependentSettings.ShowDialog = (message, action) =>
			{
				EditText et = new EditText(this);
				AlertDialog.Builder ad = new AlertDialog.Builder(this);
				ad.SetTitle(message);
				ad.SetView(et);
				ad.SetPositiveButton("Submit", (s, e) =>
				{
					action(et.Text);
				});
				ad.Show();
			};

#if DEBUG
			LoadApplication(new App());
#else
			try
			{
				LoadApplication(new App());
			}
			catch (Exception ex)
			{
				PlatformDependentSettings.ShowMessage(ex.Message);
				ReportUnhandledExceptionCallback(ex);
				throw;
			}
#endif
		}

		/// <summary>
		/// Callback to send a report about unhandled exception.
		/// </summary>
		internal static void ReportUnhandledExceptionCallback(Exception e)
		{
			string messageText = e.Message + System.Environment.NewLine + e.StackTrace;

			try
			{
				using (var sr = new System.IO.StreamReader(DataFeedClient.BasePath + Settings.SettingsFile))
					messageText += System.Environment.NewLine + sr.ReadToEnd();
			}
			catch
			{

			}

			try
			{
#pragma warning disable CS0618 // Type or member is obsolete
				SmtpClient client = new SmtpClient
#pragma warning restore CS0618 // Type or member is obsolete
				{
					Port = 587,
					EnableSsl = true,
					Credentials = new System.Net.NetworkCredential("timetablesmffuk", "timetables2018ksi"),
					Host = "smtp.gmail.com"
				};

				MailMessage mail = new MailMessage(UnhandledExceptionMailSendTo, UnhandledExceptionMailSendTo)
				{
					Subject = "Timetables Mobile Application - Unhandled exception",
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