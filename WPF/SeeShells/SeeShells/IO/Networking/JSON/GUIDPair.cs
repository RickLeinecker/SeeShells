using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SeeShells.IO.Networking.JSON
{
    public class GUIDPair
    {

        [JsonProperty(propertyName: "guid", Required = Required.Always)]
        public string GUID { private get; set; }

        [JsonProperty(propertyName: "name", Required = Required.Always)]
        public string Name { private get; set; }
        public GUIDPair(string guid, string name)
        {
            GUID = guid;
            Name = name;
        }

        public GUIDPair()
        {
        }

        public KeyValuePair<string, string> getKnownGUID()
        {
            return new KeyValuePair<string, string>(GUID, Name);
        }
    }
}
