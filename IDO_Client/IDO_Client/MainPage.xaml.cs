using IDO_Client.Controls;
using IDO_Client.Models;
using IDO_Client.Tabs;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace IDO_Client
{
    [DesignTimeVisible(true)]
    public partial class MainPage : TabbedPage
    {
        public MainPage()
        {
            InitializeComponent();
            var navigationFollowsPage = new NavigationPage(new Follows(App.Profile.Nickname, true));
            navigationFollowsPage.Icon = "follows.png";
            navigationFollowsPage.Title = "Follows";
            navigationFollowsPage.BarBackgroundColor = Color.White;

            var navigationHomePage = new NavigationPage(new Home(App.Profile, true));
            navigationHomePage.Icon = "account.png";
            navigationHomePage.Title = "Home";
            navigationHomePage.BarBackgroundColor = Color.White;

            var navigationFeedPage = new NavigationPage(new Feed(App.Profile.Nickname, true));
            navigationFeedPage.Icon = "feed.png";
            navigationFeedPage.Title = "Feed";
            navigationFeedPage.BarBackgroundColor = Color.White;

            var NavigationSettingsPAge = new CustomNavigationPage(new Settings());
            NavigationSettingsPAge.Icon = "settings.png";
            NavigationSettingsPAge.Title = "Settings";


            this.Children.Add(navigationFollowsPage);
            this.Children.Add(navigationFeedPage);
            this.Children.Add(new Idid());
            this.Children.Add(navigationHomePage);
            this.Children.Add(NavigationSettingsPAge);


        }
    }
}
