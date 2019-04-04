using IDO_Client.AccountManagementPages;
using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace IDO_Client
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();

            MainPage = new MainPage();
        }

        protected override void OnStart()
        {
            // Check for saved credentials (TO DO)

            MainPage = new CustomNavigationPage(new Login());
        }

        protected override void OnSleep()
        {
            // Handle when your app sleeps
        }

        protected override void OnResume()
        {
            // Handle when your app resumes
        }
    }
}
