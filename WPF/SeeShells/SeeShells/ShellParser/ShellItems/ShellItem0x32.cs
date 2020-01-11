using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SeeShells.ShellParser.ShellItems
{
    public class ShellItem0x32 : ShellItem0x30
    {
        public override string TypeName => "File Entry - File";
        public ShellItem0x32(byte[] buf) : base(buf) { }

    }
}
