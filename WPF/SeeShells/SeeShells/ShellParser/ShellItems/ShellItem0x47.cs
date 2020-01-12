namespace SeeShells.ShellParser.ShellItems
{
    public class ShellItem0x47 : ShellItem0x40
    {
        public override string TypeName { get => "Network Location - Entire Network"; }

        public ShellItem0x47(byte[] buf) : base(buf) { }
    }
}