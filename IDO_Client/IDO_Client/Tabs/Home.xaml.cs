using FFImageLoading.Forms;
using IDO_Client.Controls;
using IDO_Client.Models;
using Newtonsoft.Json;
using Plugin.Media;
using System;
using System.Collections.Generic;
using System.IO;
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
                Name.Text = user.Nickname;
                Followers.Text = user.Followers.Count.ToString();
                Follows.Text = user.Follows.Count.ToString();
                goals.Text = user.Goals.Count.ToString();
                base.OnPropertyChanged();
                Idid.IsCameraShowed = false;

            }
            catch
            {
                DependencyService.Get<IMessage>().LongAlert("Can't load page.");
            }
        }
        public Home(User profile, bool IsTabPage = false)
        {
            isTabPage = IsTabPage;
            user = profile;
            InitializeComponent();
            if (App.Profile.Nickname.Equals(user.Nickname))
            {
                Follow.IsEnabled = false;
                Follow.IsVisible = false;

            }
            //Name.BindingContext = user;
            //Follows.BindingContext = user;
            //Followers.BindingContext = user;
            //goals.BindingContext = user;
            //Name.SetBinding(Label.TextProperty, "Nickname");
            //Followers.SetBinding(Label.TextProperty, "Followers.Count");
            //Follows.SetBinding(Label.TextProperty, "Follows.Count");
            //goals.SetBinding(Label.TextProperty, "Goals.Count");

            if (user.Avatar != null)
            {
                avatar.Source = App.server + "/" + user.Nickname + "/" + user.Avatar + "/download";
                MiniImage.Source = App.server + "/" + user.Nickname + "/" + user.Avatar + "/download";
            }
            else
            {
                avatar.Source = "defaultavatar.jpg";
                MiniImage.Source = "defaultavatar.jpg";
            }
            

            if (App.Profile.Follows.IndexOf(user.Nickname) != -1)
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
                if (App.Profile.Follows.IndexOf(user.Nickname) == -1)
                    using (HttpClient client = new HttpClient())
                    {
                        Dictionary<string, string> values = new Dictionary<string, string>()
                            {
                                { "nickname",App.Profile.Nickname },
                                { "password", App.Profile.Password},
                                { "follow", user.Nickname}
                            };
                        var content = new FormUrlEncodedContent(values);
                        var responseReg = await client.PostAsync(App.server + "/api/accounts/follow", content);

                        if (responseReg.IsSuccessStatusCode)
                        {
                            App.Profile.Follows.Add(user.Nickname);
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
                                { "nickname",App.Profile.Nickname },
                                { "password", App.Profile.Password},
                                { "unfollow", user.Nickname}
                            };
                        var content = new FormUrlEncodedContent(values);
                        var responseReg = await client.PostAsync(App.server + "/api/accounts/unfollow", content);
                        if (responseReg.IsSuccessStatusCode)
                        {
                            App.Profile.Follows.Remove(user.Nickname);
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
            Navigation.PushAsync(new Goals(user));
        }

        private async void OnChangeAvatar(object sender, EventArgs e)
        {
            if (isTabPage)
                try
                {
                    byte[] buffer = null;
                    using (var scope = new ActivityIndicatorScope(activityIndicator, true))
                    using (HttpClient client = new HttpClient())
                    {
                        if (CrossMedia.Current.IsPickVideoSupported)
                            using (var file = await CrossMedia.Current.PickPhotoAsync())
                            {
                                if (file == null)
                                    return;
                                using (var stream = file.GetStream())
                                {
                                    avatar.Source = "load.gif";
                                    buffer = new byte[stream.Length];
                                    await stream.ReadAsync(buffer, 0, buffer.Length);
                                    avatar.Source = ImageSource.FromStream(() => new MemoryStream(buffer));
                                    MiniImage.Source = avatar.Source;
                                }
                            }
                        MultipartFormDataContent content = new MultipartFormDataContent();
                        StringContent nickname = new StringContent(App.Profile.Nickname);
                        StringContent password = new StringContent(App.Profile.Password);
                        content.Add(nickname, "nickname");
                        content.Add(password, "password");
                        if (buffer == null)
                            return;
                        ByteArrayContent baContent = new ByteArrayContent(buffer);
                        content.Add(baContent, "file", "IDID_PHOTO.jpg");
                        var response = await client.PostAsync(App.server + "/avatar", content);
                        if (response.IsSuccessStatusCode)
                        {
                            buffer = null;
                            DependencyService.Get<IMessage>().LongAlert("Succesfully uploaded!");
                        }
                        else
                        {
                            avatar.Source = "defaultavatar.jpg";
                            DependencyService.Get<IMessage>().LongAlert("Cant upload avatar");
                        }

                    }
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine(ex.Message);
                }
        }
    }
}