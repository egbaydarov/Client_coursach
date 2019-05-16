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
        Goal contextGoal { get; set; }
        User user;
        protected override void OnAppearing()
        {
            base.OnAppearing();
            try
            {
                ObservableCollection<Goal> items = new ObservableCollection<Goal>();
                GoalsView.ItemsSource = items;
                foreach (var goal in user.Goals)
                {
                    items.Add(goal);
                }
                if(items.Count == 0)
                {
                    EmptyLabel.VerticalOptions = LayoutOptions.CenterAndExpand;
                    if (user.Nickname.Equals(App.Profile.Nickname))
                    {
                        EmptyLabel.Text = "You hasn't added your goals yet.";
                        EmptyLabel.IsVisible = true;
                    }
                    else
                    {
                        EmptyLabel.Text = "This user hasn't added goals yet.";
                        EmptyLabel.IsVisible = true;
                    }
                }
                else
                {
                    EmptyLabel.IsVisible = false;
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

        private async void OnGoalTapped(object sender, EventArgs e)
        {
            if (!user.Nickname.Equals(App.Profile.Nickname))
                return;
            var goal = ((sender as Frame).BindingContext as Goal);
            if (goal.isReached)
                return;
            var idid = new Idid();
            idid.IsGoal = true;
            idid.SetGoal(goal);
            idid.SetDescription(goal.Description);
            await Navigation.PushAsync(idid);
        }

    }
}