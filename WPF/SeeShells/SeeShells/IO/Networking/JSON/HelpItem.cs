using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace SeeShells.IO.Networking.JSON
{
    public class HelpItem
    {
        [JsonProperty("id", Required = Required.Always)]
        public long Id { private get; set; }

        [JsonProperty("title", Required = Required.Always)]
        public string Title { private get; set; }

        [JsonProperty("description", Required = Required.Always)]
        public string Description { private get; set; }

        public HelpItem()
        {
        }

        public string GetHelpContent()
        {
            return Description;
        }

    }
}
