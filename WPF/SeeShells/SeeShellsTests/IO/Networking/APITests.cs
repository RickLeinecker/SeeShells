using Microsoft.VisualStudio.TestTools.UnitTesting;
using MockHttpServer;
using RestSharp;
using SeeShells.IO.Networking;
using System;
using System.Collections.Generic;
using System.IO;
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
                var apiClient = API.GetAPIClient(TEST_URL);
                Assert.IsNotNull(apiClient);
                Assert.AreEqual(testApiHost, apiClient.Result.BaseUrl.ToString());
            }
        }

        [TestMethod()]
        public void GetAPIClient_ServerErrorTest()
        {
               
            try
            {
                var apiClient = API.GetAPIClient("http://localhost:" + (TEST_PORT+1)).Result;
                Assert.Fail("Expected Exception"); //expect exception

            }
            catch (AggregateException ex)
            {
                Assert.AreEqual(new APIException("").GetType(), ex.InnerException.GetType());
            }

        }


        [TestMethod()]
        public void GetGuidsTest()
        {

            string returnJSON = File.ReadAllText(Directory.GetParent(Directory.GetCurrentDirectory()).Parent.FullName + @"\TestResource\sampleGUIDsResponse.json");
            string serializedJson = File.ReadAllText(Directory.GetParent(Directory.GetCurrentDirectory()).Parent.FullName + @"\TestResource\sampleGUIDs.json");


            using (new MockServer(TEST_PORT, API.GUID_ENDPOINT, (req, rsp, prm) => returnJSON))
            {
                var apiClient = new RestClient(TEST_URL);

                var testOutputPath = API.GetGuids("output.json", apiClient).Result;

                Assert.IsNotNull(apiClient);
                Assert.AreEqual(serializedJson, File.ReadAllText(testOutputPath));
                File.Delete(testOutputPath);
            }
        }

        [TestMethod()]
        public void GetGuids_ServerErrorTest()
        {
            string returnJSON = File.ReadAllText(Directory.GetParent(Directory.GetCurrentDirectory()).Parent.FullName + @"\TestResource\sampleAPIError.json");

            using (new MockServer(TEST_PORT, API.GUID_ENDPOINT, (req, rsp, prm) => returnJSON))
            {
                var apiClient = new RestClient(TEST_URL);

                try
                {
                    var testOutputPath = API.GetGuids("output.json", apiClient).Result;
                    Assert.Fail("Expected Exception"); //expect exception


                }
                catch (AggregateException ex)
                {
                    Assert.AreEqual(new APIException("").GetType(), ex.InnerException.GetType()); 
                }

            }

        }



        [TestMethod()]
        public void GetOSRegistryLocationsTest()
        {

            string returnJSON = File.ReadAllText(Directory.GetParent(Directory.GetCurrentDirectory()).Parent.FullName + @"\TestResource\sampleOSRegistryLocationsResponse.json");
            string serializedJSON = File.ReadAllText(Directory.GetParent(Directory.GetCurrentDirectory()).Parent.FullName + @"\TestResource\sampleOSRegistryLocations.json");

            using (new MockServer(TEST_PORT, API.OS_REGISTRY_ENDPOINT, (req, rsp, prm) => returnJSON))
            {
                var apiClient = new RestClient(TEST_URL);

                var testOutputPath = API.GetOSRegistryLocations("output.json", apiClient).Result;

                Assert.IsNotNull(apiClient);
                Assert.AreEqual(serializedJSON, File.ReadAllText(testOutputPath));
                File.Delete(testOutputPath);
            }
        }

        [TestMethod()]
        public void GetOSRegistryLocations_ServerErrorTest()
        {
            string returnJSON = File.ReadAllText(Directory.GetParent(Directory.GetCurrentDirectory()).Parent.FullName + @"\TestResource\sampleAPIError.json");

            using (new MockServer(TEST_PORT, API.OS_REGISTRY_ENDPOINT, (req, rsp, prm) => returnJSON))
            {
                var apiClient = new RestClient(TEST_URL);

                try
                {
                    var testOutputPath = API.GetOSRegistryLocations("output.json", apiClient).Result;
                    Assert.Fail("Expected Exception"); //expect exception
                }
                catch (AggregateException ex)
                {
                    Assert.AreEqual(new APIException("").GetType(), ex.InnerException.GetType());
                }

            }

        }


    }
}