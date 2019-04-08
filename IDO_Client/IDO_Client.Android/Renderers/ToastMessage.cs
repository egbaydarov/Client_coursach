using Android.App;
using Android.Widget;
using IDO_Client;
using IDO_Client.Controls;

[assembly: Xamarin.Forms.Dependency(typeof(MessageAndroid))]
namespace IDO_Client
{
    public class MessageAndroid : IMessage
    {
        public void LongAlert(string message)
        {
            Toast.MakeText(Application.Context, message, ToastLength.Long).Show();
        }

        public void ShortAlert(string message)
        {
            Toast.MakeText(Application.Context, message, ToastLength.Short).Show();
        }
    }
}