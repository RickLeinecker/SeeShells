namespace SeeShells.ShellParser.ShellItems
{
    public class ShellItem0x28 : ShellItem0x21
    {
        public override string TypeName { get => "Volume - Removable Media"; }

        public ShellItem0x28(byte[] buf) : base(buf) { }
    }
}