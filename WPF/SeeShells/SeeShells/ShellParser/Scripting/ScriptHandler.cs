using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SeeShells.ShellParser.ShellItems;

namespace SeeShells.ShellParser.Scripting
{

    public static class ScriptHandler
    {
        /// <summary>
        /// A dictionary of key value pairs to store the scripts.
        /// The key is an int for the shell item identifier.
        /// The value is a string for the content of the script.
        /// </summary>
        public static Dictionary<int, string> scripts { get; set; }

        static ScriptHandler()
        {
            scripts = new Dictionary<int, string>();
        }

        public static bool HasScriptForShellItem(int identifier)
        {
            return scripts.ContainsKey(identifier);
        }

        public static IShellItem ParseShellItem(byte[] buf, int identifier)
        {
            scripts.TryGetValue(identifier, out string script);

            return new LuaShellItem(buf, script);

        }
    }
}