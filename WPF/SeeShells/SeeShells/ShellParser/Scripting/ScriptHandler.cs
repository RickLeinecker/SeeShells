﻿using System;
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

        public ScriptHandler(string fileLocation)
        {
            GetScriptsTest();
        }

        public bool HasScriptForShellItem(int identifier)
        {
            return scripts.ContainsKey(identifier);
        }

        public IShellItem ParseShellItem(byte[] buf, int identifier)
        {
            scripts.TryGetValue(identifier, out string script);

            return new LuaShellItem(buf, script);

        }

        private void GetScripts(string fileLocation)
        {
            // get scripts from the configuration file & store them in scripts dictionary
            throw new NotImplementedException();
        }

        private void GetScriptsTest()
        {
            // see if the scripts works with a control panel shell item
            string LuaScript = @"
                    properties:Add(""TypeName"", ""Control Panel"")
                    properties:Add(""Guid"", shellitem:unpack_guid(0x0E))
                    properties:Add(""Flags"", shellitem:unpack_byte(0x03))
            ";

            scripts.Add(Int32.Parse("71", System.Globalization.NumberStyles.HexNumber), LuaScript);
        }


    }
}

