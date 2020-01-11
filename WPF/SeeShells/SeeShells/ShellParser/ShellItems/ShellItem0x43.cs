namespace SeeShells.ShellParser.ShellItems
{
    public class ShellItem0x43 : ShellItem0x40
    {
        public override string TypeName { get => "Network Location - Share UNC Path"; }

        public ShellItem0x43(byte[] buf) : base(buf) { }
    }
}