using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NLua;

namespace SeeShells.ShellParser
{
    class Script
    {
        private Lua state;
        public Script()
        {
            state = new Lua();
        }

        public int DoAdditionForMeBecauseImStupid(int x, int y)
        {
            state.DoString(@"
	            function AddFunc (val1, val2)
		            return val1 + val2;
	            end
	        ");
            LuaFunction addFunc = state["AddFunc"] as LuaFunction;
            var result = addFunc.Call(x, y).First();
            return Convert.ToInt32(result);
        }

        public int GetAShellItemPieceForMeBecauseImStupid()
        {
            ShellParser.ShellItems.ShellItem item = new ShellParser.ShellItems.ShellItem(new byte[] { 5, 0, 2, 3, 4, 5 });
            state["item"] = item;

            state.DoString(@"
	            val = item.Size;
	        ");

            var result = state["val"];
            int size = Convert.ToInt32(result);
            return size;
        }
    }
}
