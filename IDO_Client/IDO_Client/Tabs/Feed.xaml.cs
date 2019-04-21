using FFImageLoading.Forms;
using IDO_Client.Controls;
using IDO_Client.Models;
using IDO_Client.Models.Responses;
using ImageCircle.Forms.Plugin.Abstractions;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Net.Http;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace IDO_Client.Tabs
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class Feed : ContentPage
    {
        ListView _listView;
        string nickname;
        bool isFeedPage = false;
        protected async override void OnAppearing()
        {
            base.OnAppearing();
            var Data = new ObservableCollection<Data>();
            var notes = new List<Note>();
            if (isFeedPage)
            {
                foreach (var nick in App.Profile.Follows)
                    notes.AddRange(await GetUserNotes(nick));
            }
            else
            {
                notes.AddRange(await GetUserNotes(nickname));
            }
            FeedView.ItemsSource = Data;
            notes.Sort((a, b) => -a.ImageReference.CompareTo(b.ImageReference));
            foreach (var i in notes)
                Data.Add(await CreateDataModelFromNote(i, i.nickname));
            
        }
        public Feed(string nick, bool isfeedpage = false)
        {
            isFeedPage = isfeedpage;
            nickname = nick;
            InitializeComponent();
            
            //FeedView = _listView;

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
                        notes[i].nickname = nickname;
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
                    //var imageresponse = await client.GetAsync(query);
                    //if (!imageresponse.IsSuccessStatusCode)
                    //    throw new ApplicationException("Oops, Can't Download Image");
                    //ImageSource image = ImageSource.FromStream(()=> new MemoryStream((byte[])(object)imageresponse.Content));
                    string avName = (await Follows.GetUserData(nickname)).Avatar;
                    string avRef = avName != null ? $"{App.server}/{nickname}/{avName}/download": "defaultavatar.png";
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
            if (sender is ListView lv) lv.SelectedItem = null;
        }
    }
}