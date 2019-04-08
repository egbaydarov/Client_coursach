using Xamarin.Forms;
using XamarinFormsEditor.Droid.Helpers;
using Xamarin.Forms.Platform.Android;
using Android.Graphics.Drawables;
using IDO_Client.Controls;

[assembly: ExportRenderer(typeof(BorderlessEditor), typeof(BorderlessEditorRenderer))]
namespace XamarinFormsEditor.Droid.Helpers
{
#pragma warning disable CS0618 // Type or member is obsolete
    public class BorderlessEditorRenderer : EditorRenderer
    {
        protected override void OnElementChanged(ElementChangedEventArgs<Editor> e)
        {
            base.OnElementChanged(e);
            if (Control != null)
            {
                Control.Background = new ColorDrawable(Android.Graphics.Color.Transparent);
            }
        }
    }
#pragma warning restore CS0618 // Type or member is obsolete
}