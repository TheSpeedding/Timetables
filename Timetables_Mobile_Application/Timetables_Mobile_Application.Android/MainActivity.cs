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
        protected override void OnCreate(Bundle savedInstanceState)
        {
            TabLayoutResource = Resource.Layout.Tabbar;
            ToolbarResource = Resource.Layout.Toolbar;

            base.OnCreate(savedInstanceState);
            global::Xamarin.Forms.Forms.Init(this, savedInstanceState);

			// Ask for permissions if not granted yet.
			if ((int)Build.VERSION.SdkInt >= 23)
			{
				if (PackageManager.CheckPermission(Manifest.Permission.ReadExternalStorage, PackageName) != Permission.Granted &&
					PackageManager.CheckPermission(Manifest.Permission.WriteExternalStorage, PackageName) != Permission.Granted &&
					PackageManager.CheckPermission(Manifest.Permission.AccessNetworkState, PackageName) != Permission.Granted &&
					PackageManager.CheckPermission(Manifest.Permission.AccessCoarseLocation, PackageName) != Permission.Granted &&
					PackageManager.CheckPermission(Manifest.Permission.AccessFineLocation, PackageName) != Permission.Granted &&
					PackageManager.CheckPermission(Manifest.Permission.Internet, PackageName) != Permission.Granted &&
					PackageManager.CheckPermission(Manifest.Permission.AccessWifiState, PackageName) != Permission.Granted)
				{
					var permissions = new string[] {
						Manifest.Permission.ReadExternalStorage,
						Manifest.Permission.WriteExternalStorage,
						Manifest.Permission.AccessNetworkState,
						Manifest.Permission.AccessCoarseLocation,
						Manifest.Permission.AccessFineLocation,
						Manifest.Permission.Internet,
						Manifest.Permission.AccessWifiState };

					RequestPermissions(permissions, 1);
				}
			}

			PlatformDependentSettings.GetStream = (fileInfo) => Assets.Open(fileInfo.FullName.Substring(1));

			PlatformDependentSettings.SetBasePath(Android.OS.Environment.ExternalStorageDirectory.AbsolutePath + "/timetables/");

			PlatformDependentSettings.ShowMessage = (message) => Toast.MakeText(this, message, ToastLength.Long).Show();

			LoadApplication(new App());
        }
    }
}