using System;
using System.Collections.Generic;
using System.Text;


namespace SeeShells.ShellParser.ShellItems
{
    public class ShellItemList : Block
    {
        public ShellItemList(byte[] buf, int offset) : base(buf, offset) { }

        protected  IShellItem GetItem(int off)
        {
            //item to return
            IShellItem item = null;
            //the shell item type which can be identified by 2 bytes (0xXX)
            int type = unpack_byte(off + 2);

            //TODO: Aleks - Dynamically call upon correct ShellItem0xXX to be instantiated

            return item;
        }

        public IEnumerable<IShellItem> Items()
        {
            int off = offset;
            int size = 0;
            while (true)
            {
                size = unpack_word(off);

                if (size == 0)
                    break;

                IShellItem item = GetItem(off);

                size = item.Size;

                if (size > 0)
                {
                    yield return item;
                    off += size;
                }
                else
                {
                    break;
                }
            }
        }
    }
}
