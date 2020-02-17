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
        private static NLog.Logger logger = NLog.LogManager.GetCurrentClassLogger();

        public LuaShellItem(byte[] buf, string luascript): base(buf)
        {
            state = new Lua();
            state.LoadCLRPackage();
            state.DoString(@" import ('System', 'SeeShells', 'SeeShells.ShellParser.ShellItems', 'SeeShells.ShellParser') ");
            state["properties"] = properties;
            state["shellitem"] = this;
            state["knownGUIDs"] = KnownGuids.dict;

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

        public string test()
        { return "test"; }
       

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

            if (!PropertiesAreParsedAlready())
                return null;

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

            if (!PropertiesAreParsedAlready())
                return DateTime.MinValue;

            try
            {
                GetAllProperties().TryGetValue(propertyName, out string dateString);
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
                var prop = base.GetAllProperties();
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

            try
            {
                 state.DoString(@luascript);
            }
            catch (Exception ex)
            {
                // since someone might add their own scripts, they should be able to see the errors right?
                logger.Error(ex, "Error with a Lua script.\n" + ex.ToString());
            }


            state.Dispose();
        }

        // I had to grab unpack_guid and unpack_byte from the block class because
        // the functions are protected & they need to be public to be used by Lua.
        // I'm not sure what would be best to do here. Change the protected function to public
        // or make the Lua script do this itself?
        public new string unpack_guid(int off)
        {
            try
            {
                return string.Format("{0:x2}{1:x2}{2:x2}{3:x2}-{4:x2}{5:x2}-{6:x2}{7:x2}-{8:x2}{9:x2}-{10:x2}{11:x2}{12:x2}{13:x2}{14:x2}{15:x2}",
                    buf[offset + off + 3], buf[offset + off + 2], buf[offset + off + 1], buf[offset + off],
                    buf[offset + off + 5], buf[offset + off + 4],
                    buf[offset + off + 7], buf[offset + off + 6],
                    buf[offset + off + 8], buf[offset + off + 9],
                    buf[offset + off + 10], buf[offset + off + 11], buf[offset + off + 12], buf[offset + off + 13], buf[offset + off + 14], buf[offset + off + 15]);
            }
            catch (IndexOutOfRangeException ex)
            {
                throw new OverrunBufferException(offset + off, buf.Length);
            }
        }

        public new byte unpack_byte(int off)
        {
            try
            {
                return buf[offset + off];
            }
            catch (IndexOutOfRangeException ex)
            {
                throw new OverrunBufferException(offset + off, buf.Length);
            }
        }

    }
}
