using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;


namespace SeeShells.ShellParser.ShellItems
{
    public class MockShellItem : Block, IShellItem
    {
        public virtual ushort Size { get; protected set; }
        public virtual byte Type { get; protected set; }
        public virtual string TypeName { get; protected set; }
        public virtual string Name
        {
            get
            {
                return "??";
            }
            protected set
            {

            }
        }
        public virtual DateTime ModifiedDate
        {
            get
            {
                return DateTime.MinValue;
            }
            protected set
            {

            }
        }
        public virtual DateTime AccessedDate
        {
            get
            {
                return DateTime.MinValue;
            }
            protected set
            {

            }
        }
        public virtual DateTime CreationDate
        {
            get
            {
                return DateTime.MinValue;
            }
            protected set
            {

            }
        }
        [System.Obsolete("Deprecated. Use 1-parameter Constructor. Offset parameter has no effect.")]
        public MockShellItem(byte[] buf, int offset)
            : base(buf, offset)
        {
            Type = unpack_byte(0x02);
            Size = unpack_word(0x00);
        }

        public MockShellItem(byte[] buf) : base(buf, 0) {
            Type = unpack_byte(0x02);
            Size = unpack_word(0x00);
        }

        public virtual IDictionary<string, string> GetAllProperties()
        {
            SortedDictionary<string, string> properties = new SortedDictionary<string, string>();
            AddPairIfNotNull(properties, "Size", Size.ToString("X2")); //hexidecimal with 2 numerical places (aka a byte)
            AddPairIfNotNull(properties, "Type", Type.ToString("X2"));
            AddPairIfNotNull(properties, "TypeName", TypeName);
            AddPairIfNotNull(properties, "Name", Name);
            AddPairIfNotNull(properties, "ModifiedDate", ModifiedDate);
            AddPairIfNotNull(properties, "AccessedDate", AccessedDate);
            AddPairIfNotNull(properties, "CreationDate", CreationDate);
            return properties;
        }

    }
}
