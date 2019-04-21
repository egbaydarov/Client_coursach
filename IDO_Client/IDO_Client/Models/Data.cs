using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace IDO_Client.Models
{


    public class Data
    {
        


        public string Name { get; set; }
        public string Image { get; set; }
        public string Description { get; set; }
        public int LukasCount { get; set; }
        public List<string> Lukasers { get; set; }
        public List<string> ExImages { get; set; }
        public string ImageReference { get; set; }
        public string AvatarReference { get; set; }

    }



}
