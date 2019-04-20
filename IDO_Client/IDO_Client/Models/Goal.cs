using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace IDO_Client.Models
{
    public class Goal
    {
        [JsonProperty(PropertyName = "description")]
        public string Description { get; set; }
        [JsonProperty(PropertyName = "nickname")]
        public string Nickname { get; set; }
        [JsonProperty(PropertyName = "isreached")]
        public bool isReached { get; set; }
    }
}
