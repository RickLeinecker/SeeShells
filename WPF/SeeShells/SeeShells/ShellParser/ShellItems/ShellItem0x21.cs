namespace SeeShells.ShellParser.ShellItems
{
    public class ShellItem0x21 : ShellItem0x20
    {
        public override string Name { get; protected set; }
        public override string TypeName { get => "Volume - Named"; }

        public ShellItem0x21(byte[] buf) : base(buf)
        {
            Name = unpack_string(0x03);
        }
    }
}