using Plugin.Media;
using Plugin.Media.Abstractions;
using System;
using System.IO;
using System.Net.Http;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace IDO_Client.Tabs
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class Idid : ContentPage
    {
        public static bool IsCameraShowed;
        public Idid()
        {
            InitializeComponent();
        }
        protected override async void OnAppearing()
        {
            base.OnAppearing();
            if (IsCameraShowed)
                return;
            try
            {
                IsCameraShowed = true;
                if (CrossMedia.Current.IsCameraAvailable && CrossMedia.Current.IsTakePhotoSupported)
                {
                    
                    await CrossMedia.Current.Initialize();
                    using (var file = await CrossMedia.Current.TakePhotoAsync(new StoreCameraMediaOptions
                    {
                        PhotoSize = PhotoSize.Full,
                        Directory = "Photos",
                        Name = "PHOTO228.jpg"
                    }))
                    {
                        using (HttpClient client = new HttpClient())
                        using(Stream str = file.GetStream())
                        {
                            byte[] buffer = new byte[str.Length];
                            await str.ReadAsync(buffer, 0, buffer.Length);
                           
                            MultipartFormDataContent content = new MultipartFormDataContent();
                            StringContent nickname = new StringContent(App.profile.Nickname);
                            StringContent password = new StringContent(App.profile.Password);
                            content.Add(nickname, "nickname");
                            content.Add(password, "password");
                            content.Add(new StringContent("ESSSKETIT"), "description");
                            ByteArrayContent baContent = new ByteArrayContent(buffer);
                            content.Add(baContent, "file", "PHOTO228.jpg");
                            await client.PutAsync(App.server + "/api/contents", content);
                            if (file == null) return;
                            DependencyService.Get<IMessage>().ShortAlert(file.Path);
                            PhotoImage.Source = ImageSource.FromStream(() => { str.Seek(0,SeekOrigin.Begin); return str; });
                        }
                    }
                }
                else
                {
                    throw new ApplicationException("No camera access.");
                }
            }
            catch (Exception ex)
            {
                DependencyService.Get<IMessage>().ShortAlert(ex.Message);
            }
        }
    }
}