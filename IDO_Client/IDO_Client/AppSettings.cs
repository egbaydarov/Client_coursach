using System;
using System.Collections.Generic;
using System.Text;

namespace IDO_Client
{
    public class AppSettings
    {
        public AppSettings()
        {
            IsSaveToGallery = false;
        }

        public bool IsSaveToGallery { get; set;}
    }
}
