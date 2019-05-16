using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace IDO_Client
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class QuickStart : ContentPage
    {
        bool ISFirstShow;
        public QuickStart(bool isFirstShow)
        {
            ISFirstShow = isFirstShow;
            InitializeComponent();
        }

        private async void Back_clicked(object sender, EventArgs e)
        {
            if(ISFirstShow)
            App.Current.MainPage = new MainPage();
            else
            await Navigation.PopAsync();
        }
    }
}