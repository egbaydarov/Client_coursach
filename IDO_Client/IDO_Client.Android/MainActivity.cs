using System;

using Android.App;
using Android.Content.PM;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.OS;
using Plugin.CurrentActivity;
using ImageCircle.Forms.Plugin.Droid;
using FFImageLoading;

namespace IDO_Client.Droid
{


    [Activity(Label = "MOYOGOVNO", Theme = "@style/Theme.Splash", Icon = "@drawable/startpageimage", MainLauncher = true, ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation, LaunchMode = LaunchMode.SingleTop)]
    //[Activity(Label = "IDO_Client", Icon = "@mipmap/icon", Theme = "@style/MainTheme", MainLauncher = true, ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation)]
    public class MainActivity : global::Xamarin.Forms.Platform.Android.FormsAppCompatActivity
    {
        protected override void OnCreate(Bundle savedInstanceState)
        {

            FFImageLoading.Forms.Platform.CachedImageRenderer.Init(true);
            base.Window.RequestFeature(WindowFeatures.ActionBar);
            RequestedOrientation = ScreenOrientation.Portrait;
            // Name of the MainActivity theme you had there before.
            // Or you can use global::Android.Resource.Style.ThemeHoloLight
            base.SetTheme(Resource.Style.MainTheme);
            CrossCurrentActivity.Current.Init(this, savedInstanceState);
            TabLayoutResource = Resource.Layout.Tabbar;
            ToolbarResource = Resource.Layout.Toolbar;
            var ignore = typeof(FFImageLoading.Svg.Forms.SvgCachedImage);
            base.OnCreate(savedInstanceState);
            Window.SetStatusBarColor(Android.Graphics.Color.Black);
            Xamarin.Essentials.Platform.Init(this, savedInstanceState);
            global::Xamarin.Forms.Forms.Init(this, savedInstanceState);
            ImageCircleRenderer.Init();
            LoadApplication(new App());


        }
        public override void OnRequestPermissionsResult(int requestCode, string[] permissions, [GeneratedEnum] Android.Content.PM.Permission[] grantResults)
        {
            Xamarin.Essentials.Platform.OnRequestPermissionsResult(requestCode, permissions, grantResults);
            Plugin.Permissions.PermissionsImplementation.Current.OnRequestPermissionsResult(requestCode, permissions, grantResults);

            base.OnRequestPermissionsResult(requestCode, permissions, grantResults);
        }

    }
}