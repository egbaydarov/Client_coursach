using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IDO_Client.Models.Responses
{
    public class Response
    {
        
        

        [JsonProperty(PropertyName = "status")]
        public int Status { get; set; }
        public bool IsOK()
        {
            return Status == 0;
        }
        public Response(int status)
        {
            Status = status;
        }
    }
}
