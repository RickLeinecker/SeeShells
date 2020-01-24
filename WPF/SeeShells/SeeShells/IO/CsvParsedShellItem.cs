using SeeShells.ShellParser.ShellItems;
using System;
using System.Collections.Generic;

namespace SeeShells.IO
{
    class CsvParsedShellItem : IShellItem
    {
        public virtual ushort Size { get => Convert.ToUInt16(allProperties["Size"], 16); }
        public virtual byte Type { get => Convert.ToByte(allProperties["Type"], 16); }
        public virtual string TypeName { get => allProperties["TypeName"]; }
        public virtual string Name { get => allProperties["Name"]; }
        public virtual DateTime ModifiedDate { get => DateTime.Parse(allProperties["ModifiedDate"]); }
        public virtual DateTime AccessedDate { get => DateTime.Parse(allProperties["AccessedDate"]); }
        public virtual DateTime CreationDate { get => DateTime.Parse(allProperties["CreationDate"]); }

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
