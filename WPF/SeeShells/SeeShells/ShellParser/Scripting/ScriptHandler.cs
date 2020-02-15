using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SeeShells.ShellParser.ShellItems;

namespace SeeShells.ShellParser.Scripting
{

    public class ScriptHandler
    {
        /// <summary>
        /// A dictionary of key value pairs to store the scripts.
        /// The key is an int for the shell item identifier.
        /// The value is a string for the content of the script.
        /// </summary>
        private readonly Dictionary<int, string> scripts = new Dictionary<int, string>();

        public ScriptHandler()
        {
            GetScripts();
        }

        public bool HasScriptForShellItem(int identifier)
        {
            return scripts.ContainsKey(identifier);
        }

        public IShellItem ParseShellItem(int identifier)
        {
            scripts.TryGetValue(identifier, out string script);

            // make lua shell item with the script
            // return the lua shell item

            throw new NotImplementedException();
        }

        private void GetScripts()
        {
            // get scripts from the configuration file & store them in scripts dictionary
            throw new NotImplementedException();
        }


    }
}

