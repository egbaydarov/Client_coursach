using FFImageLoading.Forms;
using IDO_Client.Models;
using System;
using System.Collections.Generic;
using System.Linq;
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
            this.data = data;
            ImageView.ItemTemplate = new DataTemplate(typeof(Cell));
            List<Model> images = new List<Model>();

            images.Add(new Model(data.Image));
            if(data.ExImages != null)
            foreach(var im in data.ExImages)
                images.Add(new Model(im));
            ImageView.ItemsSource = images;
            Description.Text = data.Description;
        }

        private void OnItemTapped(object sender, ItemTappedEventArgs e)
        {
            if (e.Item == null) return;
            if (sender is ListView lv) lv.SelectedItem = null;
        }
    }
}