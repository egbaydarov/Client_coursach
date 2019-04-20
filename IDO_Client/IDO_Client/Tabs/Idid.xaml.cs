using IDO_Client.Controls;
using IDO_Client.Models.Responses;
using Newtonsoft.Json;
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
        byte[] buffer;
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
                        PhotoSize = PhotoSize.Medium,
                        SaveToAlbum = App.CurrentAppSettings.IsSaveToGallery,
                        Name = "IDID_Photo.jpg"
                    }))
                    {
                        if (file == null) return;

                        using (var scope = new ActivityIndicatorScope(activityIndicator, true))
                        using (var stream = file.GetStream())
                        {
                            PhotoImage.Source = "load.gif";
                            buffer = new byte[stream.Length];
                            await stream.ReadAsync(buffer, 0, buffer.Length);
                            PhotoImage.Source = ImageSource.FromStream(() => new MemoryStream(buffer));
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

        private void Send_Clicked(object sender, EventArgs e)
        {
            try
            {
                if (String.IsNullOrWhiteSpace(Description.Text) || buffer == null)
                    throw new ApplicationException("Oops!, Your forgot about decription or image");

                byte[] image = new byte[buffer.Length];
                buffer.CopyTo(image, 0);

                buffer = null;
                Description.Text = "";
                PhotoImage.Source = "chooseimage.png";



                using (var scope = new ActivityIndicatorScope(activityIndicator, true))
                using (HttpClient client = new HttpClient())
                {
                    MultipartFormDataContent content = new MultipartFormDataContent();
                    StringContent nickname = new StringContent(App.Profile.Nickname);
                    StringContent password = new StringContent(App.Profile.Password);
                    content.Add(nickname, "nickname");
                    content.Add(password, "password");
                    content.Add(new StringContent(Description.Text), "description");
                    ByteArrayContent baContent = new ByteArrayContent(image);
                    content.Add(baContent, "file", "IDID_PHOTO.jpg");
                    var response = client.PutAsync(App.server + "/api/contents", content);
                    response.Wait();
                    if (response.Result.IsSuccessStatusCode)
                    {
                        DependencyService.Get<IMessage>().LongAlert("Succesfully uploaded!");
                    }
                    else
                    {
                        DependencyService.Get<IMessage>().LongAlert("Cant upload achievement");
                    }

                    
                }
            }
            catch(Exception ex)
            {
                DependencyService.Get<IMessage>().LongAlert(ex.Message);
            }
        }

        private async void ChangeImage_clicked(object sender, EventArgs e)
        {
            try
            {
                if (CrossMedia.Current.IsPickVideoSupported)
                    using (var file = await CrossMedia.Current.PickPhotoAsync())
                    {
                        if (file == null)
                            return;
                        using (var stream = file.GetStream())
                        {
                            PhotoImage.Source = "load.gif";
                            buffer = new byte[stream.Length];
                            await stream.ReadAsync(buffer, 0, buffer.Length);
                            PhotoImage.Source = ImageSource.FromStream(() => new MemoryStream(buffer));
                        }
                    }
            }
            catch
            {
                DependencyService.Get<IMessage>().LongAlert("Oops, Cant load photo");
            }
        }
    }
}