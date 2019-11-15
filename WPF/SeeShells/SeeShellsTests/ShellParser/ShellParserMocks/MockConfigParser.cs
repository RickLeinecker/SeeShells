using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SeeShells.ShellParser;

namespace SeeShellsTests.ShellParser.ShellParserMocks
{
    class MockConfigParser : IConfigParser
    {
        public void GetConfig()
        {
            throw new NotImplementedException();
        }

        public List<string> GetLocations()
        {
            List<String> list = new List<String>();
            list.Add("Software\\Microsoft\\Windows\\Shell\\");
            list.Add("Software\\Microsoft\\Windows\\ShellNoRoam\\");
            list.Add("Software\\Classes\\Local Settings\\Software\\Microsoft\\Windows\\Shell");
            return list;
        }

        public void ParseConfig()
        {
            throw new NotImplementedException();
        }
    }
}
