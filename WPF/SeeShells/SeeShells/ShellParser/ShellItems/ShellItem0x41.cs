namespace SeeShells.ShellParser.ShellItems
{
    public class ShellItem0x41 : ShellItem0x40
    {
        public override string TypeName { get => "Network Location - Domain/WorkGroup Name"; }

        public ShellItem0x41(byte[] buf) : base(buf) { }
    }
}