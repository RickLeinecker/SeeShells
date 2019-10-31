using System;
using System.Collections.Generic;

using System.Text;


namespace ShellBagsParser
{
    class SHITEMLIST : Block
    {
        public SHITEMLIST(byte[] buf, int offset, object parent)
            : base(buf, offset, parent)
        {
        }

        SHITEM get_item(int off)
        {
            SHITEM item = null;
            int _type = unpack_byte(off + 2);
            if ((_type & 0x70) == (int)SHITEMTYPE.FILE_ENTRY)
            {
                try
                {
                    item = new SHITEM_FILEENTRY(buf, off, this);
                }
                catch (Exception ex)
                {
                    item = new FILEENTRY_FRAGMENT(buf, off, this, 0x04);
                }
            }
            else if (_type == (int)SHITEMTYPE.FOLDER_ENTRY)
            {
                item = new SHITEM_FOLERENTRY(buf, off, this);
            }
            else if (_type == (int)SHITEMTYPE.UNKNOWN2)
            {
                item = new SHITEM_UNKNOWNENTRY2(buf, off, this);
            }
            else if ((_type & 0x70) == (int)SHITEMTYPE.VOLUME_NAME)
            {
                item = new SHITEM_VOLUMEENTRY(buf, off, this);
            }
            else if ((_type & 0x70) == (int)SHITEMTYPE.NETWORK_LOCATION)
            {
                item = new SHITEM_NETWORKLOCATIONENTRY(buf, off, this);
            }
            else if (_type == (int)SHITEMTYPE.URI)
            {
                item = new SHITEM_URIENTRY(buf, off, this);
            }
            else if (_type == (int)SHITEMTYPE.CONTROL_PANEL)
            {
                item = new SHITEM_CONTROLPANELENTRY(buf, off, this);
            }
            else if (_type == (int)SHITEMTYPE.UNKNOWN0)
            {
                item = new SHITEM_UNKNOWNENTRY0(buf, off, this);
            }
            else if (_type == (int)SHITEMTYPE.DELETEGATE_ITEM)
            {
                item = new SHITEM_DELEGATE(buf, off, this);
            }
            else
            {
                item = new SHITEM(buf, off, this);
            }

            return item;
        }

        public IEnumerable<SHITEM> items()
        {
            int off = offset;
            int size = 0;
            while (true)
            {
                size = unpack_word(off);

                if (size == 0)
                    break;

                SHITEM item = get_item(off);

                size = item.size;

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
