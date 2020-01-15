using System;
using System.Collections.Generic;

namespace SeeShells.ShellParser.ShellItems.ExtensionBlocks
{
    public class ExtensionBlockBEEF0004 : ExtensionBlock
    {
        public DateTime CreationDate { get; protected set; }
        public DateTime AccessedDate { get; protected set; }
        public ushort LongNameSize { get; protected set; }
        public string LongName { get; protected set; }
        public string LocalizedName { get; protected set; }
        public ExtensionBlockBEEF0004(byte[] buf, int offset) : base(buf, offset)
        {
            int off = 0x08;  //pass all the known Extension Block Headers

            if (ExtensionVersion >= 0x03)
            {
                CreationDate = unpack_dosdate(off);
                off += 4;
                AccessedDate = unpack_dosdate(off);
                off += 4;
                off += 2; // unknown
            }
            else
            {
                CreationDate = DateTime.MinValue;
                AccessedDate = DateTime.MinValue;
            }

            if (ExtensionVersion >= 0x07)
            {
                off += 2;
                off += 8; // fileref
                off += 8; // unknown
            }

            if (ExtensionVersion >= 0x03)
            {
                LongNameSize = unpack_word(off);
                off += 2;
            }

            if (ExtensionVersion >= 0x09)
                off += 4; // unknown

            if (ExtensionVersion >= 0x08)
                off += 4; // unknown

            if (ExtensionVersion >= 0x03)
            {
                LongName = unpack_wstring(off);
                off += 2 * (LongName.Length + 1);
            }
            if (ExtensionVersion >= 0x03 && ExtensionVersion < 0x07 && LongNameSize > 0)
            {
                LocalizedName = unpack_string(off);
            }
            else if (ExtensionVersion >= 0x07 && LongNameSize > 0)
            {
                LocalizedName = unpack_wstring(off);
            }
            else
            {
                LocalizedName = string.Empty;
            }
        }

        public override IDictionary<string, string> GetAllProperties()
        {
            var ret = base.GetAllProperties();
            ret.Add("CreationDate", CreationDate.ToString());
            ret.Add("AccessedDate", AccessedDate.ToString());
            ret.Add("LongName", LongName.ToString());
            ret.Add("LocalizedName", LocalizedName.ToString());
            return ret;
        }
    }
}