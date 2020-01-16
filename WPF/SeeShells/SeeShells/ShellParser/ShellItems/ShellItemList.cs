using System;
using System.Collections.Generic;
using System.Text;


namespace SeeShells.ShellParser.ShellItems
{
    public class ShellItemList : Block
    {
        private static NLog.Logger logger = NLog.LogManager.GetCurrentClassLogger();

        /// <summary>
        /// Converts a RegistryKey's value into a list of usable <see cref="IShellItem"/> objects.
        /// </summary>
        /// <param name="buffer">A Byte Array retrieved from the value of a Windows Registry Key.</param>
        public ShellItemList(byte[] buffer) : base(buffer, 0) { }

        /// <summary>
        /// Identifies a ShellItem and creates a ShellItem object
        /// </summary>
        /// /// <param name="off">Marks the begining of the ShellItem</param>
        /// <returns>a ShellItem</returns>
        protected IShellItem GetItem(int off)
        {
            //the shell item type which can be identified by 2 bytes (0xXX)
            String postfix = unpack_byte(off + 2).ToString("X2");

            Type type = Type.GetType("SeeShells.ShellParser.ShellItems.ShellItem0x" + postfix);
            if(type == null)
            {
                // Getting here means that the ShellItem is unidentified
                logger.Info("Could not identify ShellItem 0x" + postfix);
                return new ShellItem(buf);

                // TODO Embedded Scripting
            }

            return (IShellItem)Activator.CreateInstance(type, buf);
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
