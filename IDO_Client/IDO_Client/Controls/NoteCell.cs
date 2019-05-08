using FFImageLoading.Forms;
using FFImageLoading.Transformations;
using FFImageLoading.Work;
using IDO_Client.Models;
using IDO_Client.Tabs;
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

        Label date = new Label { HorizontalOptions = LayoutOptions.StartAndExpand, Margin = new Thickness(7, 0, 0, 0) };

        CachedImage userAvatar = new CachedImage()
        {
            Margin = new Thickness(5, 5, 5, 5),
            VerticalOptions = LayoutOptions.CenterAndExpand,
            CacheDuration = new TimeSpan(0, 0, 30, 0),
            LoadingDelay = 50,
            Transformations = new List<ITransformation>()
            {
                new CircleTransformation(1, "#ffa6c9")
            },
            DownsampleToViewSize = true
        };
        CachedImage respectIcon = new CachedImage()
        {
            Margin = new Thickness(5, 5, 5, 5),
            VerticalOptions = LayoutOptions.CenterAndExpand,
            HorizontalOptions = LayoutOptions.EndAndExpand,
            HeightRequest = 46,
            WidthRequest = 46,
            Transformations = new List<ITransformation>()
            {
                new CircleTransformation(1, "#ffa6c9")
            },
            DownsampleToViewSize = true
        };
        Label userNicknameLabel = new Label()
        {
            Margin = new Thickness(5, 0, 0, 0),
            VerticalOptions = LayoutOptions.CenterAndExpand,
            FontSize = size * 1.25,
            TextColor = Color.Black,
            FontFamily = "Rubik-Bold.ttf#Rubik-Bold"
        };
        Label addgoalLabel = new Label()
        {
            Margin = new Thickness(0, 0, 5, 0),
            Text = "Add To Goals",
            TextColor = Color.Black,
            FontAttributes = FontAttributes.Bold,
            FontSize = size * 0.8,
            HorizontalOptions = LayoutOptions.EndAndExpand,
            FontFamily = "Rubik-Regular.ttf#Rubik-Regular"

        };
        Image additionOptionsImage = new Image()
        {
            VerticalOptions = LayoutOptions.CenterAndExpand,
            HorizontalOptions = LayoutOptions.EndAndExpand,
            HeightRequest = 46,
            WidthRequest = 46
        };
        Label progressLabel = new Label()
        {
            Margin = new Thickness(5, 0, 0, 0),
            VerticalOptions = LayoutOptions.CenterAndExpand,
            TextColor = Color.Black,
            FontAttributes = FontAttributes.Bold,
            FontSize = size
        };
        Label descriptionLabel = new Label()
        {
            Margin = new Thickness(0, 15, 0, 0),
            HeightRequest = size * 20,
            WidthRequest = size * 20,
            VerticalOptions = LayoutOptions.CenterAndExpand,
            HorizontalOptions = LayoutOptions.CenterAndExpand,
            FontSize = size * 1.5,
            TextColor = Color.White,
            FontAttributes = FontAttributes.Bold,
            FontFamily = "OpenSansCondensed-Bold.ttf#OpenSansCondensed-Bold",
            HorizontalTextAlignment = TextAlignment.Center
        };
        public readonly CachedImage achievementImage = null;
        Grid mainGrid;
        private async void OnLukased(object sender, EventArgs e)
        {
            try
            {
                using (HttpClient client = new HttpClient())
                {
                    Data data = BindingContext as Data;
                    Dictionary<string, string> values = new Dictionary<string, string>()
                    {
                        { "nickname",App.Profile.Nickname },
                        { "password", App.Profile.Password},
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
                            data.Lukasers.Add(App.Profile.Nickname);
                            progressLabel.Text = $" {likes} people think that it's cool";
                            DependencyService.Get<IMessage>().ShortAlert("You respect this!");
                        }
                        else
                        {
                            data.LukasCount--;
                            likes--;
                            data.Lukasers.Remove(App.Profile.Nickname);
                            progressLabel.Text = $" {likes} people think that it's cool";
                            DependencyService.Get<IMessage>().ShortAlert("You disrespect this!");
                        }
                        respectIcon.Source = data.Lukasers.IndexOf(App.Profile.Nickname) != -1 ? "startpageimage.png" : "unliked.png";
                    }
                }
            }
            catch
            {
                DependencyService.Get<IMessage>().ShortAlert("Something Went Wrong!");
            }
        }
        public NoteCell()
        {

            achievementImage = new CachedImage()
            {
                LoadingPlaceholder = "load.gif",
                CacheDuration = new TimeSpan(0, 0, 30, 0),
                LoadingDelay = 50,
                Transformations = new List<ITransformation>()
                {
                    new BlurredTransformation(40)
                },
                TransformPlaceholders = false,
                DownsampleWidth = Application.Current.MainPage.Width,
                //DownsampleUseDipUnits = true,
                Aspect = Aspect.AspectFill,
                VerticalOptions = LayoutOptions.CenterAndExpand
            };
            var lukasRecognizer = new TapGestureRecognizer { NumberOfTapsRequired = 2 };
            var lukasRecognizerSingle = new TapGestureRecognizer { NumberOfTapsRequired = 1 };
            lukasRecognizer.Tapped += OnLukased;
            lukasRecognizerSingle.Tapped += OnLukased;
            achievementImage.GestureRecognizers.Add(lukasRecognizer);
            respectIcon.GestureRecognizers.Add(lukasRecognizerSingle);

            var goToPostRecognizer = new TapGestureRecognizer { NumberOfTapsRequired = 1 };
            goToPostRecognizer.Tapped += OnPostTapped;
            achievementImage.GestureRecognizers.Add(goToPostRecognizer);

            var OnProfileClicked = new TapGestureRecognizer { NumberOfTapsRequired = 1 };
            OnProfileClicked.Tapped += OnProfileClickedHandler;
            userNicknameLabel.GestureRecognizers.Add(OnProfileClicked);
            userAvatar.GestureRecognizers.Add(OnProfileClicked);

            var OnLukasersCliked = new TapGestureRecognizer { NumberOfTapsRequired = 1 };
            progressLabel.GestureRecognizers.Add(OnLukasersCliked);
            OnLukasersCliked.Tapped += OnLukasersClikedHandler;

            //var showAllDiscriptionButtonImage = new Image();
            var lowerSl = new StackLayout
            {
                Orientation = StackOrientation.Horizontal
            };


            lowerSl.Children.Add(progressLabel);
            lowerSl.Children.Add(respectIcon);




            var upperSL = new StackLayout
            {
                //Padding = new Thickness(10, 6, 10, 6),
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
                    new RowDefinition { Height = size * 3},
                    new RowDefinition { Height = size * 1},
                    new RowDefinition { Height = GridLength.Auto},
                    new RowDefinition { Height = size * 2.5}
                    //new RowDefinition { Height = new GridLength(0.2,GridUnitType.Star)}
                }
            };
            mainGrid.Children.Add(upperSL, 0, 0);
            mainGrid.Children.Add(date, 0, 1);
            mainGrid.Children.Add(addgoalLabel, 0, 1);
            var imageSL = new StackLayout() {IsClippedToBounds = true };
            imageSL.Children.Add(achievementImage);
            mainGrid.Children.Add(imageSL, 0, 2);
            mainGrid.Children.Add(descriptionLabel, 0, 2);
            mainGrid.Children.Add(lowerSl, 0, 3);
            userAvatar.Source = "loading.png";

            userNicknameLabel.SetBinding(Label.TextProperty, new Binding("Name"));
            additionOptionsImage.Source = "addgoal.png";

            var goalAdd = new TapGestureRecognizer { NumberOfTapsRequired = 1 };
            goalAdd.Tapped += OnGoalsAdd_Clicked;
            additionOptionsImage.GestureRecognizers.Add(goalAdd);


            View = mainGrid;

        }

        private async void OnPostTapped(object sender, EventArgs e)
        {
            await (App.Current.MainPage as TabbedPage).CurrentPage.Navigation.PushAsync(new Post(BindingContext as Data));
        }

        private async void OnProfileClickedHandler(object sender, EventArgs e)
        {
            try
            {
                var user = BindingContext as Data;
                await (App.Current.MainPage as TabbedPage).CurrentPage.Navigation.PushAsync(new Home(await Follows.GetUserData(user.Name)));
            }
            catch (Exception ex)
            {
                DependencyService.Get<IMessage>().ShortAlert(ex.Message);
            }
        }

        private async void OnLukasersClikedHandler(object sender, EventArgs e)
        {
            var data = BindingContext as Data;
            var page = new Follows(App.Profile.Nickname);
            page.SetSearchBarIsVisible(false);
            page.isFollows = false;
            var users = new List<User>();
            foreach (var i in data.Lukasers)
                users.Add(await Follows.GetUserData(i));
            page.SetItemSource(users);
            await (App.Current.MainPage as TabbedPage).CurrentPage.Navigation.PushAsync(page);

        }

        protected override void OnBindingContextChanged()
        {
            achievementImage.Source = null;
            userAvatar.Source = null;

            base.OnBindingContextChanged();
            var item = BindingContext as Data;
            if (item == null)
            {
                return;
            }

            //WTF
            item.Name = item.Name ?? "NULL";
            item.Description = item.Description ?? "NULL";
            item.ImageReference = item.ImageReference ?? "NULL";
            item.Lukasers = item.Lukasers ?? new List<string>();

            if (item.Name.Equals(App.Profile.Nickname))
            {
                additionOptionsImage.IsEnabled = false;
                additionOptionsImage.IsVisible = false;
                addgoalLabel.IsVisible = false;
                addgoalLabel.IsEnabled = false;
            }

            achievementImage.Source = $"{App.server}/{item.Name}/{item.ImageReference}/download";
            string[] postDateArr = item.ImageReference.Split('.')[0].Split('-');
            try
            {
                date.Text = "At " + postDateArr[3] + ":" + postDateArr[4] + "  " + postDateArr[2] + "." + postDateArr[1] + "." + postDateArr[0];
            }
            catch
            {
                date.Text = "OLD_FILE";
            }
            userAvatar.Source = item.AvatarReference;
            int discriptionBound = item.Description.Length < 50 ? item.Description.Length : 50;
            descriptionLabel.Text = "_____________\n" + item.Description.Substring(0, discriptionBound) + " . . .\n_____________\nTap To View";
            respectIcon.Source = item.Lukasers.IndexOf(App.Profile.Nickname) != -1 ? "startpageimage.png" : "unliked.png";
            progressLabel.Text = $" {item.Lukasers.Count} people think that it's cool";
        }
        static NoteCell()
        {
            size = Device.GetNamedSize(NamedSize.Medium, typeof(Label));
        }
        private async void OnGoalsAdd_Clicked(object sender, EventArgs e)
        {
            try
            {
                using (HttpClient client = new HttpClient())
                {
                    Data data = BindingContext as Data;
                    Dictionary<string, string> values = new Dictionary<string, string>()
                    {
                        { "nickname",App.Profile.Nickname },
                        { "password",App.Profile.Password},
                        { "description", data.Description},
                        { "goalsnickname", data.Name}
                    };
                    var content = new FormUrlEncodedContent(values);

                    var response = await client.PostAsync(App.server + "/goals", content);
                    if (response.IsSuccessStatusCode)
                    {
                        App.Profile.Goals.Add(new Goal { Nickname = data.Name, Description = data.Description });
                        DependencyService.Get<IMessage>().ShortAlert("Goal added, check profile :)");
                    }
                    else
                        DependencyService.Get<IMessage>().ShortAlert("Sorry, Can't add goal :(");
                }
            }
            catch
            {
                DependencyService.Get<IMessage>().ShortAlert("Sorry, Can't add goal :(");
            }
        }
    }
}
