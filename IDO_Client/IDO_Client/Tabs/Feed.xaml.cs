using FFImageLoading.Forms;
using IDO_Client.Controls;
using IDO_Client.Models;
using ImageCircle.Forms.Plugin.Abstractions;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Net.Http;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.PlatformConfiguration.AndroidSpecific;
using Xamarin.Forms.Xaml;

namespace IDO_Client.Tabs
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class Feed : ContentPage
    {
        public static bool InterestingViewMode { get; set; }
        string nickname;
        bool isFeedPage = false;
        public async Task<List<Note>> GetRecommendNotes()
        {
            try
            {
                using (HttpClient client = new HttpClient())
                {
                    var response = await client.GetAsync(App.server + "/feed");
                    if (!response.IsSuccessStatusCode)
                        throw new ApplicationException("No connection to server");
                    return JsonConvert.DeserializeObject<List<Note>>(await response.Content.ReadAsStringAsync());
                }
            }
            catch (Exception e)
            {
                DependencyService.Get<IMessage>().ShortAlert(e.Message);
                return new List<Note>();
            }
        }
        public Feed(string nick, bool isfeedpage = false)
        {
            isFeedPage = isfeedpage;
            nickname = nick;
            InitializeComponent();
            Refresh();
        }
        public async Task<List<Note>> GetUserNotes(string nickname)
        {
            try
            {
                using (HttpClient client = new HttpClient())
                {
                    var response = await client.GetAsync(App.server + "/" + nickname + "/notes");
                    if (!response.IsSuccessStatusCode)
                        throw new ApplicationException("No connection to server");

                    var notesdataresponse = JsonConvert.DeserializeObject<List<Note>>(await response.Content.ReadAsStringAsync());
                    List<Note> notes = notesdataresponse;
                    for (int i = 0; i < notes.Count; i++)
                        notes[i].Nickname = nickname;
                    return notes;
                }
            }
            catch (Exception ex)
            {
                DependencyService.Get<IMessage>().ShortAlert(ex.Message);
                return new List<Note>();
            }
        }
        public async Task<Data> CreateDataModelFromNote(Note note, string nickname)
        {

            try
            {
                using (HttpClient client = new HttpClient())
                {
                    string avName = (await Follows.GetUserData(nickname)).Avatar;
                    string avRef = avName != null ? $"{App.server}/{nickname}/{avName}/download" : "defaultavatar.png";
                    var data = new Data()
                    {
                        Description = note.Description,
                        LukasCount = note.LukasCount,
                        Name = nickname,
                        Image = $"{App.server}/{nickname}/{note.ImageReference}/download",
                        Lukasers = note.Lukasers,
                        ImageReference = note.ImageReference,
                        AvatarReference = avRef,
                        ExImages = new List<string>()
                    };
                    if (note.ExImages != null)
                        foreach (var i in note.ExImages)
                            data.ExImages.Add($"{App.server}/{nickname}/{i}/download");
                    return data;
                }
            }
            catch (Exception Ex)
            {
                DependencyService.Get<IMessage>().ShortAlert(Ex.Message);
                return new Data();
            }
        }

        private void OnItemTapped(object sender, ItemTappedEventArgs e)
        {
            if (e.Item == null) return;
            if (sender is Xamarin.Forms.ListView lv) lv.SelectedItem = null;
        }

        private async void FeedView_ItemAppearing(object sender, ItemVisibilityEventArgs e)
        {
            //if (!isFeedPage)
            //    return;
            //var data = (FeedView.ItemsSource as ObservableCollection<Data>);
            //if (e.ItemIndex == data.Count - 1)
            //{
            //    var note = await GetSingleNote();
            //    data.Add(await CreateDataModelFromNote(note, note.nickname));
            //}
        }
        async void Refresh()
        {
            try
            {
                var Data = new ObservableCollection<Data>();
                var notes = new List<Note>();
                if (isFeedPage)
                {
                    foreach (var nick in App.Profile.Follows)
                        notes.AddRange(await GetUserNotes(nick));
                    if (notes.Count == 0)
                    {
                        EmptyLabel.Text = "This might be intresting . . .";
                        EmptyLabel.IsVisible = true;
                        notes.AddRange(await GetRecommendNotes());
                    }
                    else
                    {
                        EmptyLabel.IsVisible = false;
                    }

                }
                else
                {
                    notes.AddRange(await GetUserNotes(nickname));
                    if (notes.Count == 0)
                    {
                        EmptyLabel.VerticalOptions = LayoutOptions.CenterAndExpand;
                        EmptyLabel.Text = "Have no achievements there . . .";
                        EmptyLabel.IsVisible = true;
                    }
                    else
                    {
                        EmptyLabel.IsVisible = false;
                    }
                }
                FeedView.ItemsSource = Data;
                if (InterestingViewMode)
                    notes.Sort((a, b) => -a.LukasCount.CompareTo(b.LukasCount));
                else
                    notes.Sort((a, b) => -a.ImageReference.CompareTo(b.ImageReference));
                foreach (var i in notes)
                    Data.Add(await CreateDataModelFromNote(i, i.Nickname));

                FeedView.IsRefreshing = false;
            }
            catch (Exception e)
            {

                FeedView.IsRefreshing = false;
            }

        }

        private void OnRefresh(object sender, EventArgs e)
        {
            Refresh();
        }
    }
}