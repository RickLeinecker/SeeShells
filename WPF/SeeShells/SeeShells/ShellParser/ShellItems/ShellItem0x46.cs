namespace SeeShells.ShellParser.ShellItems
{
    public class ShellItem0x46 : ShellItem0x40
    {
        public override string TypeName { get => "Network Location - Microsoft Windows Network"; }

        public ShellItem0x46(byte[] buf) : base(buf) { }
    }
}