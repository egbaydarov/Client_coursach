using FFImageLoading.Forms;
using IDO_Client.Models;
using ImageCircle.Forms.Plugin.Abstractions;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using Xamarin.Forms;

namespace IDO_Client.Controls
{
    class NoteCell : ViewCell
    {
        static double size;
        Label date = new Label {HorizontalOptions = LayoutOptions.EndAndExpand };
        CircleImage userAvatar = new CircleImage()
        {
            VerticalOptions = LayoutOptions.CenterAndExpand,
            HeightRequest = 45
        };
        Label userNicknameLabel = new Label()
        {
            VerticalOptions = LayoutOptions.CenterAndExpand,
            FontSize = size + 2,
            FontAttributes = FontAttributes.Bold,
            TextColor = Color.Black
        };

        Image additionOptionsImage = new Image()
        {
            VerticalOptions = LayoutOptions.CenterAndExpand,
            HorizontalOptions = LayoutOptions.EndAndExpand,
            HeightRequest = 46
        };
        Label progressLabel = new Label()
        {
            VerticalOptions = LayoutOptions.CenterAndExpand,
            TextColor = Color.Black,
            FontAttributes = FontAttributes.Bold,
            FontSize = size
        };
        Label discriptionLabel = new Label()
        {
            VerticalOptions = LayoutOptions.CenterAndExpand,
            FontSize = size
        };
        public readonly CachedImage achievementImage = null;
        Grid mainGrid;
        private async void OnLukased(object sender, EventArgs e)
        {
            using (HttpClient client = new HttpClient())
            {
                Data data = BindingContext as Data;
                Dictionary<string, string> values = new Dictionary<string, string>()
                    {
                        { "nickname",App.profile.Nickname },
                        { "password", App.profile.Password},
                        { "note", data.ImageReference},
                        { "lukased", data.Name}
                    };
                var content = new FormUrlEncodedContent(values);

                var response = await client.PostAsync(App.server + "/lukas", content);
                if (response.IsSuccessStatusCode)
                {
                    bool isLiked = JsonConvert.DeserializeObject<bool>(await response.Content.ReadAsStringAsync());
                    int likes = int.Parse(progressLabel.Text.Split(' ')[1]);
                    if (isLiked)
                    {
                        data.LukasCount++;
                        likes++;
                        data.Lukasers.Add(App.profile.Nickname);
                        progressLabel.Text = $" {likes} people respect";
                        DependencyService.Get<IMessage>().ShortAlert("You respect this!");
                    }
                    else
                    {
                        data.LukasCount--;
                        likes--;
                        data.Lukasers.Remove(App.profile.Nickname);
                        progressLabel.Text = $" {likes} people respect";
                        DependencyService.Get<IMessage>().ShortAlert("You disrespect this!");
                    }
                    this.OnBindingContextChanged();
                }
            }
        }
        public NoteCell()
        {

            achievementImage = new CachedImage()
            {
                LoadingPlaceholder = "progress.png",
                CacheDuration = new TimeSpan(0, 0, 30, 0),
                LoadingDelay = 50
            };
            var lukasRecognizer = new TapGestureRecognizer { NumberOfTapsRequired = 2 };
            lukasRecognizer.Tapped += OnLukased;
            achievementImage.GestureRecognizers.Add(lukasRecognizer);
            
            //var showAllDiscriptionButtonImage = new Image();
            var achievementGrid = new Grid
            {
                Padding = new Thickness(5, 3, 5, 3),
                Margin = new Thickness(5, 3, 5, 3),
                RowDefinitions =
                {
                    new RowDefinition{Height = GridLength.Auto},
                    new RowDefinition{Height = GridLength.Auto}
                }
            };


            achievementGrid.Children.Add(progressLabel, 0, 0);
            achievementGrid.Children.Add(date, 0, 0);


            achievementGrid.Children.Add(discriptionLabel, 0, 1);



            var upperSL = new StackLayout
            {
                Padding = new Thickness(10, 6, 10, 6),
                Orientation = StackOrientation.Horizontal
            };

            upperSL.Children.Add(userAvatar);
            upperSL.Children.Add(userNicknameLabel);
            upperSL.Children.Add(additionOptionsImage);


            mainGrid = new Grid
            {
                RowDefinitions =
                {
                    //new RowDefinition { Height = new GridLength(0.1,GridUnitType.Star)}
                    new RowDefinition { Height = size * 2.5},
                    new RowDefinition { Height = size * 30},
                    new RowDefinition { Height = size * 4.7}
                    //new RowDefinition { Height = new GridLength(0.2,GridUnitType.Star)}
                }
            };
            mainGrid.Children.Add(upperSL, 0, 0);
            mainGrid.Children.Add(achievementImage, 0, 1);
            mainGrid.Children.Add(achievementGrid, 0, 2);
            userAvatar.Source = "loading.png";

            userNicknameLabel.SetBinding(Label.TextProperty, new Binding("Name"));
            additionOptionsImage.Source = "threedots.png";
            //achievementImage.Source = "loading.png";
            //achievementImage.SetBinding(CachedImage.SourceProperty, "Image");
            discriptionLabel.SetBinding(Label.TextProperty, new Binding("Description"));
            progressLabel.SetBinding(Label.TextProperty, "LukasCount", stringFormat: " {0} people respect");


            View = mainGrid;

        }

        protected override void OnBindingContextChanged()
        {
            achievementImage.Source = null;
            var item = BindingContext as Data;

            if (item == null)
            {
                return;
            }
            var datest = item.ImageReference.Remove(18);
            achievementImage.Source = $"{App.server}/{item.Name}/{item.ImageReference}/download";
            date.Text = "at" + datest;


            base.OnBindingContextChanged();

        }
        static NoteCell()
        {
            size = Device.GetNamedSize(NamedSize.Medium, typeof(Label));
        }
}
}
