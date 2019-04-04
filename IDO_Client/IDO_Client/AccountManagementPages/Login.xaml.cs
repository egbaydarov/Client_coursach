using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace IDO_Client.AccountManagementPages
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class Login : ContentPage
    {
        public Login()
        {
            InitializeComponent();
        }

        private void Login_Clicked(object sender, EventArgs e)
        {

        }

        private async void Register_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new Register(),true);
        }

    }
}