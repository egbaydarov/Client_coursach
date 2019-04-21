using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IDO_Client.Models
{
    public class Note
    {
        public Note(string description, string imageReference, List<string> lukasers, List<string> eximages = null, int lukascount = 0,string nick = null)
        {
            Description = description;
            ImageReference = imageReference;
            LukasCount = lukascount;
            Lukasers = lukasers;
            nickname = nick;
            ExImages = eximages;

        }
        public string nickname;
        [JsonProperty(PropertyName = "description")]
        public string Description { get; set; }

        [JsonProperty(PropertyName = "imagereference")]
        public string ImageReference { get; set; }

        [JsonProperty(PropertyName = "lukascount")]
        public int LukasCount { get; set; }

        [JsonProperty(PropertyName = "lukasers")]
        public List<string> Lukasers { get; set; }

        [JsonProperty(PropertyName = "eximages")]
        public List<string> ExImages { get; set; }

    }
}
