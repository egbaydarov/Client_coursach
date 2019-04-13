using IDO_Client.Controls;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace IDO_Client.AccountManagementPages
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class Login : ContentPage
    {
        protected override void OnAppearing()
        {
            base.OnAppearing();
            
        }
        public Login()
        {
            
            InitializeComponent();
        }

        private async void Login_Clicked(object sender, EventArgs e)
        {
            try
            {
                using (var scope = new ActivityIndicatorScope(activityIndicator, true))
                {
                    if (await App.TryLogin(UsernameEntry.Text, PasswordEntry.Text))
                    {
                        App.Current.MainPage = new MainPage();
                    }
                    else
                        throw
                            new ApplicationException("Sorry!, Wrong Nickname or Password");
                }
            }
            catch(Exception ex)
            {
                DependencyService.Get<IMessage>().ShortAlert(ex.Message);
            }
        }

        private async void Register_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new Register(),true);
        }

        
    }
}