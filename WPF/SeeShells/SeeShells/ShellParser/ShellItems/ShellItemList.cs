using System;
using System.Collections.Generic;
using System.Text;


namespace SeeShells.ShellParser.ShellItems
{
    public class ShellItemList : Block
    {
        /// <summary>
        /// Converts a RegistryKey's value into a list of usable <see cref="IShellItem"/> objects.
        /// </summary>
        /// <param name="buffer">A Byte Array retrieved from the value of a Windows Registry Key.</param>
        public ShellItemList(byte[] buffer) : base(buffer, 0) { }

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
