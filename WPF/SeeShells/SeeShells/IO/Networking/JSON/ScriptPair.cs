using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace SeeShells.IO.Networking.JSON
{
    public class ScriptPair
    {

        [JsonProperty(propertyName: "identifier", Required = Required.Always)]
        public int Identifier { private get; set; }

        [JsonProperty(propertyName: "script", Required = Required.Always)]
        public string Script { private get; set; }
        public ScriptPair(int identifier, string script)
        {
            Identifier = identifier;
            Script = script;
        }

        public ScriptPair() { }

        public KeyValuePair<int, string> getScript()
        {
            return new KeyValuePair<int, string>(Identifier, Script);
        }
    }
}
