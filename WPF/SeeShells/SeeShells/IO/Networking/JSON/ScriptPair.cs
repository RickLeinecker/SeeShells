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

        private static NLog.Logger logger = NLog.LogManager.GetCurrentClassLogger();

        public int typeidentifier { get; set; }

        private string decodedScript;
        public string script { 
            get { return decodedScript; }
            set
            {
                decodedScript = Base64Decode(value);
            }
        }

        public ScriptPair() { }

        public KeyValuePair<int, string> getScript()
        {
            return new KeyValuePair<int, string>(typeidentifier, decodedScript);
        }

        private static string Base64Decode(string base64EncodedData)
        {
            var base64EncodedBytes = Convert.FromBase64String(base64EncodedData);
            return Encoding.UTF8.GetString(base64EncodedBytes);
        }
    }
}
