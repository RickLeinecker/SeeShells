using System;
using System.Collections.Generic;
using System.Text;


namespace SeeShells.ShellParser.ShellItems
{
    [Obsolete("Legacy Code meant for diff baseline checking only.")]
    class SHITEMLIST : Block
    {
        public SHITEMLIST(byte[] buf, int offset, object parent)
            : base(buf, offset)
        {
        }

        ShellItem get_item(int off)
        {
            ShellItem item = null;
            int _type = unpack_byte(off + 2);
            if ((_type & 0x70) == (int)SHITEMTYPE.FILE_ENTRY)
            {
                try
                {
                    item = new ShellItem0x30(buf);
                }
                catch (Exception ex)
                {
                    item = new FILEENTRY_FRAGMENT(buf, off, this, 0x04);
                }
            }
            else if (_type == (int)SHITEMTYPE.FOLDER_ENTRY)
            {
                item = new ShellItem0x1F(buf);
            }
            else if (_type == (int)SHITEMTYPE.UNKNOWN2)
            {
                item = new SHITEM_UNKNOWNENTRY2(buf, off, this);
            }
            else if ((_type & 0x70) == (int)SHITEMTYPE.VOLUME_NAME)
            {
                item = new ShellItem0x20(buf);
            }
            else if ((_type & 0x70) == (int)SHITEMTYPE.NETWORK_LOCATION)
            {
                item = new ShellItem0x40(buf);
            }
            else if (_type == (int)SHITEMTYPE.URI)
            {
                item = new ShellItem0x61(buf);
            }
            else if (_type == (int)SHITEMTYPE.CONTROL_PANEL)
            {
                item = new ShellItem0x71(buf);
            }
            else if (_type == (int)SHITEMTYPE.UNKNOWN0)
            {
                item = new ShellItem0x00(buf);
            }
            else if (_type == (int)SHITEMTYPE.DELETEGATE_ITEM)
            {
                item = new ShellItem0x74(buf);
            }
            else
            {
                item = new ShellItem(buf, off);
            }

            return item;
        }

        public IEnumerable<ShellItem> items()
        {
            int off = offset;
            int size = 0;
            while (true)
            {
                size = unpack_word(off);

                if (size == 0)
                    break;

                ShellItem item = get_item(off);

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
