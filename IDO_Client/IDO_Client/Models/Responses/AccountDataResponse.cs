using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IDO_Client.Models.Responses
{
    public class AccountDataResponse : Response
    {

        [JsonProperty(PropertyName = "data")]
        public User Data { get; set; }
        public AccountDataResponse(int status, User data) : base(status)
        {
            Data = data;
        }
    }
}
