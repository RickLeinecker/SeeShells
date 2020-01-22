using SeeShells.ShellParser.ShellItems;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SeeShellsTests.UI.Mocks
{
    class MockShellItem : IShellItem
    {
        public MockShellItem(string name, byte type)
        {
            Name = name;
            Type = type;
        }
        public MockShellItem()
        {
            Name = "";
            Type = 0x99;
        }

        public ushort Size {get; set;}

        public byte Type {get; set;}

        public string TypeName {get; set;}

        public string Name {get; set;}

        public DateTime ModifiedDate {get; set;}

        public DateTime AccessedDate {get; set;}

        public DateTime CreationDate {get; set;}

        public IDictionary<string, string> GetAllProperties()
        {
            return new Dictionary<string, string>()
            {
                {"Name", Name },
                {"Type", Type.ToString("X2") },
                {"thing1", "thing2" },
                {"thing3", "thing4" },
                {"thing5", "thing6" },

            };
        }
    }
}
