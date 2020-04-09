using SeeShells.ShellParser.ShellItems;
using System;
using System.Collections.Generic;

namespace SeeShells.IO
{
    public class CsvParsedShellItem : IShellItem
    {
        public virtual ushort Size { get => Convert.ToUInt16(allProperties[Constants.SIZE], 16); }
        public virtual byte Type { get => Convert.ToByte(allProperties[Constants.TYPE], 16); }
        public virtual string TypeName { get => allProperties[Constants.TYPENAME]; }
        public virtual string Name { get => allProperties[Constants.NAME]; }
        public virtual DateTime ModifiedDate { get => DateTime.Parse(allProperties[Constants.MODIFIED_DATE]); }
        public virtual DateTime AccessedDate { get => DateTime.Parse(allProperties[Constants.ACCESSED_DATE]); }
        public virtual DateTime CreationDate { get => DateTime.Parse(allProperties[Constants.CREATION_DATE]); }

        IDictionary<string, string> allProperties;

        public CsvParsedShellItem(IDictionary<string, string> allProperties)
        {
            this.allProperties = allProperties;
        }

        public virtual IDictionary<string, string> GetAllProperties()
        {
            return allProperties;
        }
    }
}
