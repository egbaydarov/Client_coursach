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

namespace IDO_Client.Tabs
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class Settings : ContentPage
    {
        public Settings()
        {
            InitializeComponent();
            SaveGalarySwitcher.BindingContext = App.CurrentAppSettings;
            SaveGalarySwitcher.SetBinding(Switch.IsToggledProperty, "IsSaveToGallery");
        }
        protected override void OnAppearing()
        {
            base.OnAppearing();
            Idid.IsCameraShowed = false;
        }
        private void LogOut_clicked(object sender, EventArgs e)
        {
            App.Current.Properties.Remove("password");
            App.Current.Properties.Remove("nickname");
            App.Current.MainPage = new CustomNavigationPage(new Login());
            App.profile = null;
        }

        private void Change_clicked(object sender, EventArgs e)
        {
            using (HttpClient client = new HttpClient())
            {
                
            }
        }

        private void OnSaveGallery(object sender, ToggledEventArgs e)
        {
            if (App.Current.Properties.Keys.Contains("IsSaveToGallery"))
                App.Current.Properties["IsSaveToGallery"] = SaveGalarySwitcher.IsToggled;
            else
                App.Current.Properties.Add("IsSaveToGallery", SaveGalarySwitcher.IsToggled);
        }
    }
}