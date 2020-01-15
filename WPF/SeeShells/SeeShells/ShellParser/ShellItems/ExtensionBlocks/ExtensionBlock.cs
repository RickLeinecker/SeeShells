using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SeeShells.ShellParser.ShellItems.ExtensionBlocks
{

    public class ExtensionBlock : Block, IExtensionBlock
    {

        public virtual ushort Size { get; protected set; }
        public virtual ushort ExtensionVersion { get; protected set; }
        public virtual uint Signature { get; protected set; }


        public ExtensionBlock(byte[] buf, int offset) : base (buf, offset)
        {
            Size = unpack_word(0x00);
            ExtensionVersion = unpack_word(0x02);
            Signature = unpack_dword(0x04);
        }

        public virtual IDictionary<string, string> GetAllProperties()
        {
            SortedDictionary<string, string> properties = new SortedDictionary<string, string>()
            {
                {"Size", Size.ToString("X2")}, //hexidecimal with 2 numerical places (aka a byte)
                {"ExtensionVersion", ExtensionVersion.ToString() },
                {"Signature", Signature.ToString("X4") },
            };
            return properties;
        }
    }
}
