using SeeShells.ShellParser;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SeeShellsTests.ShellParser.ShellParserMocks
{
    class OfflineMockConfigParser : IConfigParser
    {
        public void GetConfig()
        {
            throw new NotImplementedException();
        }

        public List<string> GetLocations()
        {
            List<String> list = new List<String>();
            list.Add(@"Software\Microsoft\Windows\Shell");
            return list;
        }

        public void ParseConfig()
        {
            throw new NotImplementedException();
        }
    }
}
