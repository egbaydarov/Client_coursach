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
        //public const string server = @"https://idoapi20190409055028.azurewebsites.net/";
        public const string Alphabet = "QWERTYUIOPASDFGHJKLZXCVBNMqwertyuiopasdfghjklzzxcvbnm1234567890<>_-";

        public static User profile;
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
                    ServicePointManager.ServerCertificateValidationCallback += (sendere, cert, chain, sslPolicyErrors) => true;
                    var responseJS = await client.GetAsync(server+ "/account" + "/" + login + "/" + pass);
                    string content = await responseJS.Content.ReadAsStringAsync();
                    var response = JsonConvert.DeserializeObject<AccountDataResponse>(content);
                    
                    if (response.IsOK())
                    {
                        profile = response.Data;
                        object nick;
                        object passs;
                        if (!App.Current.Properties.TryGetValue("nickname", out nick) && !App.Current.Properties.TryGetValue("password", out passs))
                        {
                            App.Current.Properties.Add("nickname", profile.Nickname);
                            App.Current.Properties.Add("password", profile.Password);
                        }
                        await App.Current.SavePropertiesAsync();
                        DependencyService.Get<IMessage>().ShortAlert("Succesfull!");
                        return true;
                    }
                    else
                    {
                        DependencyService.Get<IMessage>().ShortAlert("Oops!");
                        return false;
                    }
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
                {
                    MainPage = new MainPage();
                }
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
