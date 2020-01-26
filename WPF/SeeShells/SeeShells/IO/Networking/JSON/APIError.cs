using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SeeShells.IO.Networking.JSON
{
    /// <summary>
    /// C# object representation of an API Call which returned error information instead of it's intended information.
    /// </summary>
    public class APIError
    {
        [JsonProperty(propertyName:"success", Required = Required.Always)]
        public int Success { get; set; }
        [JsonProperty(propertyName:"error", Required = Required.Always)]
        public string Error { get; set; }

        public APIError(int success, string error)
        {
            Success = success;
            Error = error; ;
        }

        public APIError()
        {
        }
    }
}
