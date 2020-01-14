namespace SeeShells.ShellParser.ShellItems
{
    public class ShellItem0x42 : ShellItem0x40
    {
        public override string TypeName { get => "Network Location - Server UNC Path"; }

        public ShellItem0x42(byte[] buf) : base(buf) { }
    }
}