using IDO_Client.Controls;
using IDO_Client.Models;
using IDO_Client.Tabs;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace IDO_Client
{
    // Learn more about making custom code visible in the Xamarin.Forms previewer
    // by visiting https://aka.ms/xamarinforms-previewer
    [DesignTimeVisible(true)]
    public partial class MainPage : TabbedPage
    {
        public MainPage()
        {
            InitializeComponent();
            this.Children.Add(new Follows(App.profile.Nickname));
            this.Children.Add(new Feed(App.profile.Nickname));
            this.Children.Add(new Idid());
            this.Children.Add(new Home(App.profile));
            this.Children.Add(new Settings());
            
        }

        

        private void OnPAgeChanged_Handler(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            


        }
    }
}
