using IDO_Client.AccountManagementPages;
using IDO_Client.Controls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace IDO_Client.EditPages
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class EditProfile : ContentPage
    {
        bool CorrectDataKey;
        public EditProfile()
        {
            InitializeComponent();
        }

        private async void SaveChanges_Clicked(object sender, EventArgs e)
        {
            try
            {
                if (CorrectDataKey)
                    using (var scope = new ActivityIndicatorScope(activityIndicator, true))
                    using (HttpClient client = new HttpClient())
                    {
                        string newPass = PassFirst.Text;
                        if ((PassFirst.Text == null) || PassFirst.Text.Equals(""))
                            newPass = App.Profile.Password;
                        Dictionary<string, string> values = new Dictionary<string, string>()
                        {
                        { "newnickname",newNickname.Text },
                        { "nickname", App.Profile.Nickname },
                        { "password", curPass.Text},
                        { "newpassword", newPass}
                        };
                        var content = new FormUrlEncodedContent(values);
                        var response = await client.PutAsync(App.server + "/api" + "/accounts", content);
                        if (!response.IsSuccessStatusCode)
                            throw new ApplicationException("Wrong old assword or nickname already taken.");
                        if (await App.TryLogin(newNickname.Text, newPass))
                        {
                            return;
                        }
                    }
                else
                    throw new ApplicationException("Incorrect data for changes.");
            } 
            catch (Exception ex)
            {
                DependencyService.Get<IMessage>().ShortAlert(ex.Message);
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

        private void OnIncorrectPass(object sender, TextChangedEventArgs e)
        {
            Entry entry = (Entry)(sender);
            if (entry.Text == null || entry.Text.Equals(""))
            {
                CorrectDataKey = true;
                return;
            }
            if (entry.Text.IndexOf(' ') != -1 || entry.Text.Length < 6 || !entry.Text.IsStringConsistOf(App.Alphabet + "!@#$%^&*()" + "1234567890"))
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

        private void OnIncorrectCurrentPass(object sender, TextChangedEventArgs e)
        {

        }

        private void OnIncorrectNickname(object sender, TextChangedEventArgs e)
        {
            Entry entry = (Entry)(sender);
            if (entry.Text.IndexOf(' ') != -1 || entry.Text.Length < 4 || !entry.Text.IsStringConsistOf(App.Alphabet))
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