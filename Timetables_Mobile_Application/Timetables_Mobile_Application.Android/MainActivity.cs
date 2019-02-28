using System;

using Android.App;
using Android.Content.PM;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.OS;
using Android;

namespace Timetables.Application.Mobile.Droid
{
    [Activity(Label = "Timetables", Icon = "@mipmap/icon", Theme = "@style/MainTheme", MainLauncher = true, ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation)]
    public class MainActivity : global::Xamarin.Forms.Platform.Android.FormsAppCompatActivity
    {
		public override void OnRequestPermissionsResult(int requestCode, string[] permissions, Android.Content.PM.Permission[] grantResults)
		{
			Plugin.Permissions.PermissionsImplementation.Current.OnRequestPermissionsResult(requestCode, permissions, grantResults);
			base.OnRequestPermissionsResult(requestCode, permissions, grantResults);
		}
		protected override void OnCreate(Bundle savedInstanceState)
        {
            TabLayoutResource = Resource.Layout.Tabbar;
            ToolbarResource = Resource.Layout.Toolbar;

            base.OnCreate(savedInstanceState);
            global::Xamarin.Forms.Forms.Init(this, savedInstanceState);
			
			var permissions = new string[] {
						Manifest.Permission.ReadExternalStorage,
						Manifest.Permission.WriteExternalStorage,
						Manifest.Permission.AccessNetworkState,
						Manifest.Permission.AccessCoarseLocation,
						Manifest.Permission.AccessFineLocation,
						Manifest.Permission.Internet,
						Manifest.Permission.AccessWifiState,
						Manifest.Permission.AccessMockLocation,
						Manifest.Permission.AccessLocationExtraCommands
			};

			foreach (var p in permissions)
			{
				if ((int)Build.VERSION.SdkInt >= 23)
				{
					if (PackageManager.CheckPermission(p, PackageName) != Permission.Granted)
					{
						RequestPermissions(new string[] { p }, 1);
					}
				}
			}

			Xamarin.FormsGoogleMaps.Init(this, savedInstanceState);

			PlatformDependentSettings.GetStream = (fileInfo) => Assets.Open(fileInfo.FullName.Substring(1));

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

			LoadApplication(new App());
        }
    }
}