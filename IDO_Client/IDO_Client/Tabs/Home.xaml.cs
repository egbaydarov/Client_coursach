using IDO_Client.Controls;
using IDO_Client.Models;
using Newtonsoft.Json;
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
    public partial class Home : ContentPage
    {
        User user;
        bool isTabPage;
        protected override void OnAppearing()
        {
            try
            {


                base.OnAppearing();
                Idid.IsCameraShowed = false;
            }
            catch
            {
                DependencyService.Get<IMessage>().LongAlert("Can't Load Page");
            }
        }
        public Home(User profile, bool IsTabPage = false)
        {
            isTabPage = IsTabPage;
            user = profile;
            InitializeComponent();
            if (App.profile.Nickname.Equals(user.Nickname))
            {
                Follow.IsEnabled = false;
                Follow.IsVisible = false;

            }
            Name.BindingContext = user;
            Follows.BindingContext = user;
            Followers.BindingContext = user;
            Name.SetBinding(Label.TextProperty, "Nickname");
            Followers.SetBinding(Label.TextProperty, "Followers.Count");
            Follows.SetBinding(Label.TextProperty, "Follows.Count");
            if (App.profile.Follows.IndexOf(user.Nickname) != -1)
            {
                Follow.Text = "Unfollow";
            }

        }

        private async void OnFollowersClicked(object sender, EventArgs e)
        {
            using (var scope = new ActivityIndicatorScope(activityIndicator, true))
            {
                var FollowersPage = new Follows(user.Nickname);
                FollowersPage.isFollows = false;
                List<User> users = new List<User>();

                FollowersPage.SetSearchBarIsVisible(false);
                foreach (var nick in user.Followers)
                {
                    users.Add(await Tabs.Follows.GetUserData(nick));
                }

                NavigationPage.SetHasNavigationBar(FollowersPage, true);
                FollowersPage.SetItemSource(users);
                await Navigation.PushAsync(FollowersPage);
            }
        }

        private async void OnFollowsClicked(object sender, EventArgs e)
        {
            using (var scope = new ActivityIndicatorScope(activityIndicator, true))
            {
                var FollowsPage = new Follows(user.Nickname);
                FollowsPage.isFollows = false;
                List<User> users = new List<User>();

                FollowsPage.SetSearchBarIsVisible(false);
                foreach (var nick in user.Follows)
                {
                    users.Add(await Tabs.Follows.GetUserData(nick));
                }

                NavigationPage.SetHasNavigationBar(FollowsPage, true);
                FollowsPage.SetItemSource(users);
                await Navigation.PushAsync(FollowsPage);
            }
        }

        private async void OnFollowClicked(object sender, EventArgs e)
        {
            try
            {
                if (App.profile.Follows.IndexOf(user.Nickname) == -1)
                    using (HttpClient client = new HttpClient())
                    {
                        Dictionary<string, string> values = new Dictionary<string, string>()
                    {
                        { "nickname",App.profile.Nickname },
                        { "password", App.profile.Password},
                        { "follow", user.Nickname}
                    };
                        var content = new FormUrlEncodedContent(values);
                        var responseReg = await client.PostAsync(App.server + "/api/accounts/follow", content);

                        if (responseReg.IsSuccessStatusCode)
                        {
                            App.profile.Follows.Add(user.Nickname);
                            Follow.Text = "Unfollow";
                        }
                        else
                            throw new ApplicationException("Can't follow.");
                    }
                else
                    using (HttpClient client = new HttpClient())
                    {
                        Dictionary<string, string> values = new Dictionary<string, string>()
                    {
                        { "nickname",App.profile.Nickname },
                        { "password", App.profile.Password},
                        { "unfollow", user.Nickname}
                    };
                        var content = new FormUrlEncodedContent(values);
                        var responseReg = await client.PostAsync(App.server + "/api/accounts/unfollow", content);
                        if (responseReg.IsSuccessStatusCode)
                        {
                            App.profile.Follows.Remove(user.Nickname);
                            Follow.Text = "Follow";

                        }
                        else
                            throw new ApplicationException("Can't unfollow.");
                    }
            }
            catch
            {
            }
        }

        private async void OnAchievementsClicked(object sender, EventArgs e)
        {
            try
            {

                var page = new Feed(user.Nickname);
                NavigationPage.SetHasNavigationBar(page, true);
                await Navigation.PushAsync(page);
            }
            catch
            {
            }

        }

        private void OnGoalsClicked(object sender, EventArgs e)
        {

        }

        private void OnChangeAvatar(object sender, EventArgs e)
        {

        }
    }
}