using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.Graphics;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using IDO_Client.Controls;
using IDO_Client.Droid.Renderers;
using Xamarin.Forms;

[assembly: Xamarin.Forms.Dependency(typeof(ImageInfo))]
namespace IDO_Client.Droid.Renderers
{

    public class ImageInfo : IImageInfo
    {

        public Tuple<int, int> GetFileWidthAndHeight(string file)
        {

            BitmapFactory.Options options = new BitmapFactory.Options()
            {
                InJustDecodeBounds = true
            };
            options.InJustDecodeBounds = true;

            BitmapFactory.DecodeFile(file, options);
            var resId = Forms.Context.Resources.GetIdentifier(file, "drawable", Forms.Context.PackageName);
            BitmapFactory.DecodeResource(Forms.Context.Resources, resId, options);
            int width = options.OutWidth;
            int height = options.OutHeight;

            return new Tuple<int, int>(width, height);
        }
    }

}