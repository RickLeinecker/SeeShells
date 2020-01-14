using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SeeShells.ShellParser.ShellItems
{
    public class ShellItem0x31 : ShellItem0x30
    {
        public override string TypeName => "File Entry - Directory";
        public ShellItem0x31(byte[] buf) : base(buf) { }

    }
}
