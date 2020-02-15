using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SeeShells.ShellParser.ShellItems;
using NLua;

namespace SeeShells.ShellParser.Scripting
{
    public class LuaShellItem : IShellItem
    {
        private readonly Lua state = new Lua();

        public ushort Size => throw new NotImplementedException();

        public byte Type => throw new NotImplementedException();

        public string TypeName => throw new NotImplementedException();

        public string Name => throw new NotImplementedException();

        public DateTime ModifiedDate => throw new NotImplementedException();

        public DateTime AccessedDate => throw new NotImplementedException();

        public DateTime CreationDate => throw new NotImplementedException();

        public IDictionary<string, string> GetAllProperties()
        {
            throw new NotImplementedException();
        }
    }
}
