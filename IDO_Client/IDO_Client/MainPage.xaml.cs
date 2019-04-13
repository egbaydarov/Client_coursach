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
    // Learn more about making custom code visible in the Xamarin.Forms previewer
    // by visiting https://aka.ms/xamarinforms-previewer
    [DesignTimeVisible(true)]
    public partial class MainPage : TabbedPage
    {
        public MainPage()
        {
            InitializeComponent();
            var navigationFollowsPage = new NavigationPage(new Follows(App.profile.Nickname));
            navigationFollowsPage.Icon = "follows.png";
            navigationFollowsPage.Title = "Follows";
            navigationFollowsPage.BarBackgroundColor = Color.White;
            var navigationHomePage = new NavigationPage(new Home(App.profile, true));
            navigationHomePage.Icon = "account.png";
            navigationHomePage.Title = "Home";
            navigationHomePage.BarBackgroundColor = Color.White;
            this.Children.Add(navigationFollowsPage);
            this.Children.Add(new Feed(App.profile.Nickname, true));
            this.Children.Add(new Idid());
            this.Children.Add(navigationHomePage);
            this.Children.Add(new Settings());
            
        }
    }
}
