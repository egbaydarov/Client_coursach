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
    public partial class SetServer : ContentPage
    {
        public SetServer()
        {
            InitializeComponent();
            Server.Text = App.server;
        }

        private void SetServer_Clicked(object sender, EventArgs e)
        {
            App.server = Server.Text;
        }
    }
}