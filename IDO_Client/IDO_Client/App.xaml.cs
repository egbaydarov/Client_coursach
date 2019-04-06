using IDO_Client.AccountManagementPages;
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
        public const string Alphabet = "QWERTYUIOPASDFGHJKLZXCVBNMqwertyuiopasdfghjklzzxcvbnm<>_-";



        public App()
        {
            InitializeComponent();

        }
        public static User profile; 
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
        protected override void OnStart()
        {
            // Check for saved credentials (TO DO)

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
