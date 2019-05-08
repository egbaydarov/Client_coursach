using IDO_Client.AccountManagementPages;
using IDO_Client.Controls;
using IDO_Client.EditPages;
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
        }
        private void LogOut_clicked(object sender, EventArgs e)
        {
            App.Current.Properties.Remove("password");
            App.Current.Properties.Remove("nickname");
            App.Current.MainPage = new CustomNavigationPage(new Login());
            App.Profile = null;
        }

        private async void Change_clicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new EditProfile());
        }

        private async void OnSaveGallery(object sender, ToggledEventArgs e)
        {
            if (App.Current.Properties.Keys.Contains("IsSaveToGallery"))
                App.Current.Properties["IsSaveToGallery"] = SaveGalarySwitcher.IsToggled;
            else
                App.Current.Properties.Add("IsSaveToGallery", SaveGalarySwitcher.IsToggled);
            await App.Current.SavePropertiesAsync();
        }

        private async void QuikStart_clicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new QuickStart());
        }

        private async void About_clicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new About());
        }

        private async void Feedmode_clicked(object sender, EventArgs e)
        {
            if (Feed.InterestingViewMode)
                feedMode.Text = "Feed Mode: New";
            else
                feedMode.Text = "Feed Mode: Interesting";
            

            Feed.InterestingViewMode = !Feed.InterestingViewMode;

            if (App.Current.Properties.Keys.Contains("InterestingFeedMode"))
                App.Current.Properties["InterestingFeedMode"] = Feed.InterestingViewMode;
            else
                App.Current.Properties.Add("InterestingFeedMode", Feed.InterestingViewMode);
            await App.Current.SavePropertiesAsync();
        }
    }
}