using FFImageLoading.Forms;
using IDO_Client.Controls;
using IDO_Client.Models;
using ImageCircle.Forms.Plugin.Abstractions;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace IDO_Client.Tabs
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class Follows : ContentPage
    {
        string Nickname;
        public bool isFollows = true;
        public void SetItemSource(List<User> users)
        {
            ObservableCollection<User> items = new ObservableCollection<User>(users);
            FollowsView.ItemsSource = items;
        }
        public void SetSearchBarIsVisible(bool isvisivle)
        {
            searchBar.IsEnabled = isvisivle;
            searchBar.IsVisible = isvisivle;
        }
        protected async override void OnAppearing()
        {
            base.OnAppearing();
            if (isFollows)
            try
            {
                ObservableCollection<User> items = new ObservableCollection<User>();
                var User = await GetUserData(Nickname);
                FollowsView.ItemsSource = items;
                foreach (var nick in User.Follows)
                {
                    items.Add(await GetUserData(nick));
                }
                
            }
            catch
            {
                DependencyService.Get<IMessage>().LongAlert("Oops, Can't Load Follows");
            }
        }
        public Follows(string nickname)
        {
            InitializeComponent();
            Nickname = nickname;

        }

       static public async Task<User> GetUserData(string Nickname)
        {
            using (HttpClient client = new HttpClient())
            {
                var response = await client.GetAsync(App.server + "/account/" + Nickname);
                return JsonConvert.DeserializeObject<User>(await response.Content.ReadAsStringAsync());
            }
        }

        private async void OnSearchDataChanged(object sender, TextChangedEventArgs e)
        {
            if (String.IsNullOrWhiteSpace(e.NewTextValue))
            {

                ObservableCollection<User> items = new ObservableCollection<User>();
                var User = await GetUserData(Nickname);
                FollowsView.ItemsSource = items;
                foreach (var nick in User.Follows)
                {
                    items.Add(await GetUserData(nick));
                }
            }
            try
            {
                using (var scope = new ActivityIndicatorScope(activityIndicator, true))
                using (HttpClient client = new HttpClient())
                {
                    var response = await client.GetAsync(App.server + $"/searchuser/{e.NewTextValue.ToLower()}");
                    ObservableCollection<User> items = new ObservableCollection<User>(JsonConvert.DeserializeObject<List<User>>(await response.Content.ReadAsStringAsync()));
                    FollowsView.ItemsSource = items;
                }
                
            }
            catch
            {
            }
        }

        private async void OnProfileTapped(object sender, EventArgs e)
        {
            try
            {
                var user = (sender as Frame).BindingContext as User;
                await Navigation.PushAsync(new Home(await GetUserData(user.Nickname)));
                
            }
            catch (Exception ex)
            {
                DependencyService.Get<IMessage>().ShortAlert(ex.Message);
            }
        }

        private void OnItemTapped(object sender, ItemTappedEventArgs e)
        {
            if (e.Item == null) return;
            if (sender is ListView lv) lv.SelectedItem = null;
        }

        private void AvatarContxt_changed(object sender, EventArgs e)
        {
            var item = ( sender as CachedImage).BindingContext as User;
            var image = sender as CachedImage;
            if(item != null)
            image.Source = item.Avatar != null ? $"{App.server}/{item.Nickname}/{item.Avatar}/download" : "defaultavatar.jpg" ;
        }
    }
}