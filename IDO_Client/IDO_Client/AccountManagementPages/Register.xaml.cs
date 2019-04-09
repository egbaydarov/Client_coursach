using IDO_Client.Controls;
using IDO_Client.Models.Responses;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace IDO_Client.AccountManagementPages
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class Register : ContentPage
    {
        bool CorrectDataKey;
        public Register()
        {
            InitializeComponent();
        }

        private async void Register_Clicked(object sender, EventArgs e)
        {
            try
            {
                using (var scope = new ActivityIndicatorScope(activityIndicator, true))
                using (var client = new HttpClient())
                {
                    ServicePointManager.ServerCertificateValidationCallback +=
                     (sendere, cert, chain, sslPolicyErrors) => true;
                    if (CorrectDataKey)
                    {
                        if (!PassFirst.Text.Equals(PassSecond.Text))
                            throw new ApplicationException("Passwords should be the same.");
                        Dictionary<string, string> values = new Dictionary<string, string>()
                    {
                        { "nickname",Nick.Text },
                        { "password", PassFirst.Text}
                    };
                        var content = new FormUrlEncodedContent(values);
                        var responseReg = await client.PostAsync(App.server + "/api" + "/accounts", content);
                        var response = JsonConvert.DeserializeObject<SimpleResponse>(await responseReg.Content.ReadAsStringAsync());
                        if (response.IsOK())
                        {
                            if (await App.TryLogin(Nick.Text, PassFirst.Text))
                            {
                                App.Current.MainPage = new MainPage();
                            }
                            else
                            {
                                DependencyService.Get<IMessage>().LongAlert("Soory!, Can't login");
                            }
                        }
                        else
                        {
                            DependencyService.Get<IMessage>().LongAlert(response.Message);
                        }
                    }
                    else
                    {
                        DependencyService.Get<IMessage>().LongAlert("Sorry!, Too Weak Password or login, try more different symbols");
                    }
                }
            }
            catch (Exception ex)
            {
                await DisplayAlert("Oops", ex.Message, "It's Clear.");
            }
        }

        private void OnIncorrectLogin(object sender, TextChangedEventArgs e)
        {
            Entry entry = (Entry)(sender);
            if (entry.Text.IndexOf(' ') != -1 || entry.Text.Length < 6 || !entry.Text.IsStringConsistOf(App.Alphabet))
            {
                entry.BackgroundColor = Color.FromHex("#ff6d6d");
                CorrectDataKey = false;
            }
            else
            {
                entry.BackgroundColor = Color.Default;
                CorrectDataKey = true;
            }
        }

        private void OnIncorrectPass(object sender, TextChangedEventArgs e)
        {
            Entry entry = (Entry)(sender);
            if (entry.Text.IndexOf(' ') != -1 || entry.Text.Length < 6 || entry.Text.IndexOfAny("!@#$%^&*()".ToCharArray()) == -1 || !entry.Text.IsStringConsistOf(App.Alphabet + "!@#$%^&*()" + "1234567890") || entry.Text.IndexOfAny("1234567890".ToCharArray()) == -1)
            {
                entry.BackgroundColor = Color.FromHex("#ff6d6d");
                CorrectDataKey = false;
            }
            else
            {
                entry.BackgroundColor = Color.Default;
                CorrectDataKey = true;
            }
        }

        private void OnIncorrectPassAgain(object sender, TextChangedEventArgs e)
        {
            Entry entry = (Entry)(sender);
            if (!entry.Text.Equals(PassFirst.Text))
            {
                entry.BackgroundColor = Color.FromHex("#ff6d6d");
                CorrectDataKey = false;
            }
            else
            {
                entry.BackgroundColor = Color.Default;
                CorrectDataKey = true;
            }
        }
    }
}