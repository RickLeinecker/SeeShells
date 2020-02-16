using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SeeShells.ShellParser.ShellItems;
using NLua;

namespace SeeShells.ShellParser.Scripting
{
    public class LuaShellItem : ShellItem
    {
        private readonly Lua state;
        private readonly string luascript;
        Dictionary<string, string> properties = new Dictionary<string, string>();

        public LuaShellItem(byte[] buf, string luascript): base(buf)
        {
            state = new Lua();
            state.LoadCLRPackage();
            state.DoString(@" import ('SeeShells', 'SeeShells.ShellParser.ShellItems') ");
            state["propeties"] = properties;
            state["shellitem"] = this;

            this.luascript = luascript;
        }

        public override string TypeName
        { 
            get
            {
                return GetString("TypeName");
            }
        }


        public override string Name
        {
            get
            {
                return GetString("Name");
            }
        }

        public override DateTime ModifiedDate
        {
            get
            {
                return GetDate("ModifiedDate");
            }
        }

        public override DateTime AccessedDate
        {
            get
            {
                return GetDate("AccessedDate");
            }
        }

        public override DateTime CreationDate
        {
            get
            {
                return GetDate("CreationDate");
            }
        }

        private string GetString(string propertyName)
        {
            string result;

            try
            {
                GetAllProperties().TryGetValue(propertyName, out string typename);
                result = typename;
            }
            catch (ArgumentNullException)
            {
                result = "";
            }
            
            return result;
        }

        private DateTime GetDate(string propertyName)
        {
            DateTime date;
            GetAllProperties().TryGetValue(propertyName, out string dateString);

            try
            {
                date = DateTime.Parse(dateString);
            }
            catch (ArgumentNullException)
            {
                date = DateTime.MinValue;
            }
            catch (FormatException)
            {
                date = DateTime.MinValue;
            }

            return date;
        }

        public override IDictionary<string, string> GetAllProperties()
        {
            if(!PropertiesAreParsedAlready())
            {
                ParseShellItem();
            }

            return properties;
        }

        private bool PropertiesAreParsedAlready()
        {
            if (properties.Count > 0)
                return true;

            return false;
        }

        private void ParseShellItem()
        {
            if (luascript is null || luascript == "")
                throw new Exception("No Lua script provided");

            // better way to check for validity?
            if (!luascript.Contains("properties:Add"))
                throw new Exception("There is no properties table in this Lua script to get information from");

            state.DoString(@luascript);

            state.Dispose();
        }

    }
}
