using IDO_Client.Controls;
using IDO_Client.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace IDO_Client.Tabs
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class Goals : ContentPage
    {
        User user;
        protected override void OnAppearing()
        {
            base.OnAppearing();
            Idid.IsCameraShowed = false;
            try
            {
                ObservableCollection<Goal> items = new ObservableCollection<Goal>();

                GoalsView.ItemsSource = items;
                foreach (var goal in user.Goals)
                {
                    items.Add(goal);
                }

            }
            catch
            {
                DependencyService.Get<IMessage>().LongAlert("Oops, Can't Load Goals");
            }
        }
        public Goals(User user)
        {
            this.user = user;
            InitializeComponent();
        }

        private void OnItemTapped(object sender, ItemTappedEventArgs e)
        {
            if (e.Item == null) return;
            if (sender is ListView lv) lv.SelectedItem = null;
        }

        private void OnGoalTapped(object sender, EventArgs e)
        {

        }
    }
}