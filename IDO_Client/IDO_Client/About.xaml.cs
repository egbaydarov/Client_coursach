using IDO_Client.Controls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace IDO_Client
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class About : ContentPage
    {
        public About()
        {
            InitializeComponent();
        }

        private async void ReportBug(object sender, EventArgs e)
        {
            try
            {
                if (String.IsNullOrWhiteSpace(message.Text) || message.Text.Length < 1)
                {
                    DependencyService.Get<IMessage>().ShortAlert("Incorect data in entry.");
                    return;
                }
                string deviceInfo = "";
                deviceInfo += "Device Name: " + DeviceInfo.Name;
                deviceInfo += " DeviceType: " + DeviceInfo.DeviceType.ToString();
                deviceInfo += " Platform: " + DeviceInfo.Platform;
                deviceInfo += " Version: " + DeviceInfo.Version;
                deviceInfo += " Idiom: " + DeviceInfo.Idiom;
                deviceInfo += " Model: " + DeviceInfo.Model;
                deviceInfo += " Manufacturer: " + DeviceInfo.Manufacturer;
                deviceInfo += " " +DateTime.Now;
                using (HttpClient client = new HttpClient())
                {
                    Dictionary<string, string> values = new Dictionary<string, string>()
                        {
                            { "message","REPORT:" + message.Text + deviceInfo}
                        };
                    var content = new FormUrlEncodedContent(values);
                    var result = await client.PostAsync(App.server + "/report", content);
                    if(result.IsSuccessStatusCode)
                    {
                        DependencyService.Get<IMessage>().ShortAlert("Report sended!");
                        message.Text = "";
                    }
                    else
                    {
                        throw new ApplicationException("Cant send report.");
                    }
                }
            }
            catch (Exception ex)
            {
                DependencyService.Get<IMessage>().ShortAlert(ex.Message);
            }
        }
    }
}