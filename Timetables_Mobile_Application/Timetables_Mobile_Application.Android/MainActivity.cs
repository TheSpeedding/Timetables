using System;

using Android.App;
using Android.Content.PM;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.OS;
using Android;
using Android.Content;

namespace Timetables.Application.Mobile.Droid
{
	[Activity(Label = "Timetables", Icon = "@mipmap/icon", Theme = "@style/MainTheme", MainLauncher = true, ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation)]
    public class MainActivity : global::Xamarin.Forms.Platform.Android.FormsAppCompatActivity
    {
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

			PlatformDependentSettings.SetBasePath(Android.OS.Environment.ExternalStorageDirectory.AbsolutePath + "/timetables/");

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

			bool firstCall = false; // ConnectivityTypeChanged event is broken, it's called twice everytime the type is changed.

			Plugin.Connectivity.CrossConnectivity.Current.ConnectivityTypeChanged += async (s, e) =>
			{
				firstCall = !firstCall;

				if (!firstCall) return;

				bool isWifi = false;
				
				foreach (var connectionType in e.ConnectionTypes)
					if (connectionType == Plugin.Connectivity.Abstractions.ConnectionType.WiFi)
						isWifi = true;

				if (isWifi)
					try
					{
						await Request.UpdateCachedResultsAsync(true);
					}
					catch { }
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
				throw;
			}
#endif
		}
    }
}