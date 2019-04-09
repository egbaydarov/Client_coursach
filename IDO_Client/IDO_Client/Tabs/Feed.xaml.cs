using IDO_Client.Controls;
using IDO_Client.Models;
using IDO_Client.Models.Responses;
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
        string nickname;
        protected async override void OnAppearing()
        {
            base.OnAppearing();

            var Data = new ObservableCollection<Data>();
            var notes = await GetUserNotes(nickname, 0, 0);

            FeedView.ItemsSource = Data;
            foreach (var i in notes)
                Data.Add(CreateDataModelFromNote(i, nickname));


            Idid.IsCameraShowed = false;
        }
        public Feed(string nick)
        {
            nickname = nick;
            InitializeComponent();


        }
        public async Task<List<Note>> GetUserNotes(string nickname, int from, int to)
        {
            try
            {
                using (HttpClient client = new HttpClient())
                {
                    var response = await client.GetAsync(App.server + "/" + nickname + "/notes");
                    var notesdataresponse = JsonConvert.DeserializeObject<NotesDataResponse>(await response.Content.ReadAsStringAsync());
                    if (!notesdataresponse.IsOK())
                        throw new ApplicationException("Oops, Can't load notes");
                    List<Note> notes = notesdataresponse.Notes;
                    return notes;
                }
            }
            catch (Exception ex)
            {
                DependencyService.Get<IMessage>().ShortAlert(ex.Message);
                return new List<Note>();
            }
        }
        public Data CreateDataModelFromNote(Note note, string nickname)
        {

            try
            {
                using (HttpClient client = new HttpClient())
                {
                    //var imageresponse = await client.GetAsync(query);
                    //if (!imageresponse.IsSuccessStatusCode)
                    //    throw new ApplicationException("Oops, Can't Download Image");
                    //ImageSource image = ImageSource.FromStream(()=> new MemoryStream((byte[])(object)imageresponse.Content));

                    var data = new Data()
                    {
                        Description = note.Description,
                        LukasCount = note.LukasCount,
                        Name = nickname,
                        Image = $"{App.server}/{nickname}/{note.ImageReference}/download",
                        Lukasers = note.Lukasers
                    };
                    return data;
                }
            }
            catch (Exception Ex)
            {
                DependencyService.Get<IMessage>().ShortAlert(Ex.Message);
                return new Data();
            }
        }

        private void Back_Clicked(object sender, EventArgs e)
        {
            try
            {
                App.Current.MainPage = new MainPage();
            }
            catch (Exception)
            {

            }
        }
    }
}