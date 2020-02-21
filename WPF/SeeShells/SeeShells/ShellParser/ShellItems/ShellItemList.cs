using System;
using SeeShells.ShellParser.Scripting;
using System.Collections.Generic;

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
            if (type == null || type == Type.GetType("SeeShells.ShellParser.ShellItems.ShellItem0x71"))
            { 
                // if we have a script for the ShellItem, use it to get the information needed
                int identifier = unpack_byte(off + 2);
                if (ScriptHandler.HasScriptForShellItem(identifier))
                {
                    return ScriptHandler.ParseShellItem(buf, identifier);
                }

                // Getting here means that the ShellItem is unidentified
                logger.Info("Could not identify ShellItem 0x" + postfix);
                return new ShellItem(buf);
            }

            try
            {
                return (IShellItem)Activator.CreateInstance(type, buf);
            }
            catch(Exception ex)
            {
                logger.Error(ex, "ShellItem0x" + postfix + " Failed to create\n" + "ShellItem Byte array:\n" + BitConverter.ToString(buf) + "\n" + ex.ToString());
            }
            return new ShellItem(buf);
        }

        public IEnumerable<IShellItem> Items()
        {
            int off = offset;
            int size = 0;
            while (true)
            {
                //prevent out of bounds reading when the exact size of an item meets the next expected offset
                if (size == off && off != 0)
                    break;

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
