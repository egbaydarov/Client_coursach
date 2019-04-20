using IDO_Client.AccountManagementPages;
using IDO_Client.Controls;
using IDO_Client.Models;
using IDO_Client.Models.Responses;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace IDO_Client
{
    public partial class App : Application
    {
        public const string server = @"http://192.168.1.39:44374";
        //public const string server = @"https://idoapi20190414023717.azurewebsites.net";
        public const string Alphabet = "QWERTYUIOPASDFGHJKLZXCVBNMqwertyuiopasdfghjklzzxcvbnm1234567890<>_-";

        public static User Profile;
        public static AppSettings CurrentAppSettings { get; set; }


        public App()
        {
            InitializeComponent();

            CurrentAppSettings = new AppSettings();
            object IsSaveToGallery;
            if (App.Current.Properties.TryGetValue("IsSaveToGalary", out IsSaveToGallery))
            {
                CurrentAppSettings.IsSaveToGallery = (bool)IsSaveToGallery;
            }
            MainPage = new LoadPage();

        }
        public static async Task<bool> TryLogin(string login, string pass)
        {
            try
            {
                using (var client = new HttpClient())
                {
                    var responseJS = await client.GetAsync(server + "/account" + "/" + login + "/" + pass);

                    if (!responseJS.IsSuccessStatusCode)
                    {
                        DependencyService.Get<IMessage>().ShortAlert("Oops!");
                        return false;
                    }
                    string content = await responseJS.Content.ReadAsStringAsync();
                    var response = JsonConvert.DeserializeObject<User>(content);

                    object nick;
                    object passs;
                    if (!App.Current.Properties.TryGetValue("nickname", out nick) && !App.Current.Properties.TryGetValue("password", out passs))
                    {
                        App.Current.Properties.Add("nickname", login);
                        App.Current.Properties.Add("password", pass);
                    }
                    Profile = response;
                    Current.Properties["nickname"] = login;
                    Current.Properties["password"] = pass;
                    await App.Current.SavePropertiesAsync();
                    
                    Profile.Password = pass;
                    DependencyService.Get<IMessage>().ShortAlert("Succesfull!");
                    return true;
                }
            }
            catch (Exception e)
            {
                Console.Error.WriteLine(e.Message);
                return false;
            }
        }
        protected async override void OnStart()
        {

            object nick;
            object pass;
            if (App.Current.Properties.TryGetValue("nickname", out nick) && App.Current.Properties.TryGetValue("password", out pass))
            {
                bool isLogined = await App.TryLogin(nick as string, pass as string);
                if (isLogined)
                    MainPage = new MainPage();
                else
                    MainPage = new CustomNavigationPage(new Login());
            }
            else
                MainPage = new CustomNavigationPage(new Login());
        }

        protected override void OnSleep()
        {
            // Handle when your app sleeps
        }

        protected override void OnResume()
        {
            // Handle when your app resumes
        }
    }
}
