using FFImageLoading.Forms;
using IDO_Client.Controls;
using IDO_Client.Models;
using Plugin.Media;
using Plugin.Media.Abstractions;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
    public partial class Post : ContentPage
    {
        Data data;
        class Cell : ViewCell
        {
            CachedImage image = new CachedImage()
            {
                CacheDuration = new TimeSpan(0, 30, 0),
                DownsampleWidth = Application.Current.MainPage.Width,
                DownsampleUseDipUnits = true,
                Aspect = Aspect.AspectFill,
                VerticalOptions = LayoutOptions.CenterAndExpand
            };
            StackLayout Sl = new StackLayout() { IsClippedToBounds = true };

            public Cell()
            {
                Sl.Children.Add(image);
                View = Sl;
            }

            protected override void OnBindingContextChanged()
            {
                image.Source = null;
                base.OnBindingContextChanged();
                var item = BindingContext as Model;
                if (item == null)
                {
                    return;
                }
                image.Source = item.ImSourse;
            }

        }

        class Model
        {
            public ImageSource ImSourse { get; set; }

            public Model(ImageSource image)
            {
                ImSourse = image;
            }
        }
        public Post(Data data)
        {
            InitializeComponent();
            if (data.Name.Equals(App.Profile.Nickname))
            {
                edit.IsVisible = true;
                edit.IsEnabled = true;
            }
            this.data = data;
            ImageView.ItemTemplate = new DataTemplate(typeof(Cell));
            images.Add(new Model(data.Image));
            if (data.ExImages != null)
                foreach (var im in data.ExImages)
                    images.Add(new Model(im));
            ImageView.ItemsSource = images;
            Description.Text = data.Description;
        }

        private void OnItemTapped(object sender, ItemTappedEventArgs e)
        {
            if (e.Item == null) return;
            if (sender is ListView lv) lv.SelectedItem = null;
        }

        private async void OnDeleteTapped(object sender, EventArgs e)
        {

            using (var scope = new ActivityIndicatorScope(activityIndicator, true))
            using (var client = new HttpClient())
                try
                {
                    var response = await client.DeleteAsync(App.server + $"/delete/{App.Profile.Nickname}/{App.Profile.Password}/{data.ImageReference}");
                    if (!response.IsSuccessStatusCode)
                        DependencyService.Get<IMessage>().LongAlert("Oops, Cant delete achievement.");
                    else
                    {
                        DependencyService.Get<IMessage>().ShortAlert("Achievement deleted!");
                        await Navigation.PopAsync();
                    }
                }
                catch (Exception ex)
                {
                    DependencyService.Get<IMessage>().ShortAlert(ex.Message);
                }
        }


        ObservableCollection<Model> images = new ObservableCollection<Model>();
        byte[] buffer;
        byte[] buffer1;
        byte[] buffer2;
        byte[] buffer3;




        private async void OnAddImageTapped(object sender, EventArgs e)
        {
            try
            {
                if (images.Count > 4)
                {
                    DependencyService.Get<IMessage>().ShortAlert("4 photo limit.");
                    return;
                }
                using (var scope = new ActivityIndicatorScope(activityIndicator, true))
                    if (CrossMedia.Current.IsPickVideoSupported)
                        using (var file = await CrossMedia.Current.PickPhotoAsync())
                        {
                            if (file == null)
                                return;
                            using (var stream = file.GetStream())
                            {
                                switch (images.Count)
                                {
                                    case 0:
                                        buffer = new byte[stream.Length];
                                        await stream.ReadAsync(buffer, 0, buffer.Length);
                                        images.Add(new Model(ImageSource.FromStream(() => new MemoryStream(buffer))));
                                        break;
                                    case 1:
                                        buffer1 = new byte[stream.Length];
                                        await stream.ReadAsync(buffer1, 0, buffer1.Length);
                                        images.Add(new Model(ImageSource.FromStream(() => new MemoryStream(buffer1))));
                                        break;
                                    case 2:
                                        buffer2 = new byte[stream.Length];
                                        await stream.ReadAsync(buffer2, 0, buffer2.Length);
                                        images.Add(new Model(ImageSource.FromStream(() => new MemoryStream(buffer2))));
                                        break;
                                    case 3:
                                        buffer3 = new byte[stream.Length];
                                        await stream.ReadAsync(buffer3, 0, buffer3.Length);
                                        images.Add(new Model(ImageSource.FromStream(() => new MemoryStream(buffer3))));
                                        break;
                                    default:
                                        break;
                                };

                            }
                        }

            }
            catch
            {
                DependencyService.Get<IMessage>().LongAlert("Oops, Cant load photo");
            }
        }

        private void OnRemoveImageTapped(object sender, EventArgs e)
        {

            switch (images.Count)
            {
                case 1:
                    buffer = null;
                    images.RemoveAt(images.Count - 1);
                    break;
                case 2:
                    buffer1 = null;
                    images.RemoveAt(images.Count - 1);
                    break;
                case 3:
                    buffer2 = null;
                    images.RemoveAt(images.Count - 1);
                    break;
                case 4:
                    buffer3 = null;
                    images.RemoveAt(images.Count - 1);
                    break;
                default:
                    break;
            }
        }

        private async void OnSendTapped(object sender, EventArgs e)
        {
            using (var scope = new ActivityIndicatorScope(activityIndicator, true))
                try
                {
                    byte[] image = null;
                    byte[] image1 = null;
                    byte[] image2 = null;
                    byte[] image3 = null;
                    if (String.IsNullOrWhiteSpace(Description.Text) || buffer == null)
                        throw new ApplicationException("Oops!, Your forgot about decription or image");
                    if (buffer != null)
                    {
                        image = new byte[buffer.Length];
                        buffer.CopyTo(image, 0);
                    }
                    if (buffer1 != null)
                    {
                        image1 = new byte[buffer1.Length];
                        buffer1.CopyTo(image1, 0);
                    }
                    if (buffer2 != null)
                    {
                        image2 = new byte[buffer2.Length];
                        buffer2.CopyTo(image2, 0);
                    }
                    if (buffer3 != null)
                    {
                        image3 = new byte[buffer3.Length];
                        buffer3.CopyTo(image3, 0);
                    }
                    buffer = null;
                    buffer1 = null;
                    buffer2 = null;
                    buffer3 = null;
                    string descr = Description.Text.Clone() as string;
                    Description.Text = "";

                    images.Clear();

                    using (HttpClient client = new HttpClient())
                    {
                        MultipartFormDataContent content = new MultipartFormDataContent();
                        StringContent nickname = new StringContent(App.Profile.Nickname);
                        StringContent password = new StringContent(App.Profile.Password);
                        content.Add(nickname, "nickname");
                        content.Add(password, "password");
                        content.Add(new StringContent(descr), "description");
                        if (image != null)
                        {
                            ByteArrayContent baContent = new ByteArrayContent(image);
                            content.Add(baContent, "file", "IDID_PHOTO.jpg");
                        }
                        else
                        {
                            DependencyService.Get<IMessage>().LongAlert("Add Image Please.");
                            return;
                        }
                        if (image1 != null)
                        {
                            ByteArrayContent baContent = new ByteArrayContent(image1);
                            content.Add(baContent, "file1", "IDID_PHOTO.jpg");
                        }
                        if (image2 != null)
                        {
                            ByteArrayContent baContent = new ByteArrayContent(image2);
                            content.Add(baContent, "file2", "IDID_PHOTO.jpg");
                        }
                        if (image3 != null)
                        {
                            ByteArrayContent baContent = new ByteArrayContent(image3);
                            content.Add(baContent, "file3", "IDID_PHOTO.jpg");
                        }
                        var response1 = await client.DeleteAsync(App.server + $"/delete/{App.Profile.Nickname}/{App.Profile.Password}/{data.ImageReference}");
                        if (!response1.IsSuccessStatusCode)
                        {
                            DependencyService.Get<IMessage>().LongAlert("Oops, Can't Update acievement.");
                            return;
                        }
                        var response = await client.PutAsync(App.server + "/api/contents", content);
                        if (response.IsSuccessStatusCode)
                        {
                            DependencyService.Get<IMessage>().LongAlert("Succesfully uploaded!");
                        }
                        else
                        {
                            DependencyService.Get<IMessage>().LongAlert("Cant upload achievement");
                        }
                    }
                }
                catch (Exception ex)
                {
                    DependencyService.Get<IMessage>().LongAlert(ex.Message);
                }
        }

        private async void OnCameraTapped(object sender, EventArgs e)
        {
            try
            {
                if (images.Count > 4)
                {
                    DependencyService.Get<IMessage>().ShortAlert("4 photo limit.");
                    return;
                }

                using (var scope = new ActivityIndicatorScope(activityIndicator, true))
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
                            if (file == null)
                                return;
                            using (var stream = file.GetStream())
                            {
                                switch (images.Count)
                                {
                                    case 0:
                                        buffer = new byte[stream.Length];
                                        await stream.ReadAsync(buffer, 0, buffer.Length);
                                        images.Add(new Model(ImageSource.FromStream(() => new MemoryStream(buffer))));
                                        break;
                                    case 1:
                                        buffer1 = new byte[stream.Length];
                                        await stream.ReadAsync(buffer1, 0, buffer1.Length);
                                        images.Add(new Model(ImageSource.FromStream(() => new MemoryStream(buffer1))));
                                        break;
                                    case 2:
                                        buffer2 = new byte[stream.Length];
                                        await stream.ReadAsync(buffer2, 0, buffer2.Length);
                                        images.Add(new Model(ImageSource.FromStream(() => new MemoryStream(buffer2))));
                                        break;
                                    case 3:
                                        buffer3 = new byte[stream.Length];
                                        await stream.ReadAsync(buffer3, 0, buffer3.Length);
                                        images.Add(new Model(ImageSource.FromStream(() => new MemoryStream(buffer3))));
                                        break;
                                    default:
                                        break;
                                };
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

        private void OnEditTapped(object sender, EventArgs e)
        {
            edit.IsVisible = false;
            edit.IsEnabled = false;
            EditStack.IsVisible = true;
            EditStack.IsEnabled = true;
            Description.IsReadOnly = false;
            images.Clear();
            
        }
    }
}