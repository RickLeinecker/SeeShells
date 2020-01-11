namespace SeeShells.ShellParser.ShellItems
{
    public class ShellItem0x4E : ShellItem0x40
    {
        public override string TypeName { get => "Network Location - NetworkPlaces"; }

        public ShellItem0x4E(byte[] buf) : base(buf) { }
    }
}