using IDO_Client.AccountManagementPages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace IDO_Client.Tabs
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class Settings : ContentPage
    {
        public Settings()
        {
            InitializeComponent();
        }

        private void LogOut_clicked(object sender, EventArgs e)
        {
            App.Current.MainPage = new CustomNavigationPage(new Login());
            App.profile = null;
        }
    }
}