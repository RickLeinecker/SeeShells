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
            GetScriptsTest();
        }

        public void GetScripts(string fileLocation)
        {
            // get scripts from the configuration file & store them in scripts dictionary
            throw new NotImplementedException();
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

        private void GetScriptsTest()
        {
            // see if the scripts works with a control panel shell item
            string LuaScript = @"         
                local guid = shellitem:unpack_guid(0x0E)
                local flag = shellitem:unpack_byte(0x03)

                properties:Add(""TypeName"", ""Control Panel"")
                properties:Add(""Guid"", tostring(guid))
                properties:Add(""Flags"", tostring(flag))

                if knownGUIDs:ContainsKey(tostring(guid)) then
                    properties:Add(""Name"", string.format(""{{CONTROL PANEL: %s}}"", knownGUIDs[guid]))
                else
                    properties:Add(""Name"", string.format(""{{CONTROL PANEL: %s}}"", guid))
                end
            ";

            scripts.Add(Int32.Parse("71", System.Globalization.NumberStyles.HexNumber), LuaScript);
        }


    }
}

