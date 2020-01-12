using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SeeShells.ShellParser.ShellItems
{
    public class ShellItem0x34 : ShellItem0x30
    {
        public override string TypeName => "File Entry - Unicode";
        public ShellItem0x34(byte[] buf) : base(buf) { }

    }
}
