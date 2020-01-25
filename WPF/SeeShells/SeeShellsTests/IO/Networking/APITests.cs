using Microsoft.VisualStudio.TestTools.UnitTesting;
using MockHttpServer;
using SeeShells.IO.Networking;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SeeShellsTests.IO.Networking
{
    [TestClass()]
    public class APITests
    {
        static readonly int TEST_PORT = 4935; //dont use port 80 because the testing system might already have a webserver
        static readonly string TEST_URL = $"http://localhost:{TEST_PORT}";


        [TestMethod()]
        public void GetAPIClientTest()
        {

            string testApiHost = "http://localhost/api";

            using (new MockServer(TEST_PORT, API.DEFAULT_WEBSITE_API_ENDPOINT, (req, rsp, prm) => testApiHost))
            {
                var apiClient = API.GetAPIClientAsync(TEST_URL);
                Assert.IsNotNull(apiClient);
                Assert.AreEqual(testApiHost, apiClient.Result.BaseUrl.ToString());
            }
        }
    }
}