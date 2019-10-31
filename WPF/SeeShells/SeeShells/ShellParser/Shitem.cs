using System;
using System.Collections.Generic;
using System.Text;


namespace SeeShells.ShellParser
{
    class SHITEM : Block
    {
        public virtual ushort size { get; protected set; }
        public virtual byte type { get; protected set; }
        public virtual string name
        {
            get
            {
                return "??";
            }
            protected set
            {

            }
        }
        public virtual DateTime m_date
        {
            get
            {
                return DateTime.MinValue;
            }
            protected set
            {

            }
        }
        public virtual DateTime a_date
        {
            get
            {
                return DateTime.MinValue;
            }
            protected set
            {

            }
        }
        public virtual DateTime cr_date
        {
            get
            {
                return DateTime.MinValue;
            }
            protected set
            {

            }
        }
        public SHITEM(byte[] buf, int offset, object parent)
            : base(buf, offset, parent)
        {
            type = unpack_byte(0x02);
            size = unpack_word(0x00);
        }

    }
}
