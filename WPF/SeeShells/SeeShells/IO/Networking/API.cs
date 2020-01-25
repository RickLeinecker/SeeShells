using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SeeShells.IO.Networking
{
    public class API
    {
        private static NLog.Logger logger = NLog.LogManager.GetCurrentClassLogger();
        
        private static IRestClient apiClient = null;

        public const string DEFAULT_WEBSITE_URL = "https://rickleinecker.github.io/SeeShells";
        public const string DEFAULT_WEBSITE_API_ENDPOINT = "apiEndpoint.txt";


        /// <summary>
        /// Obtains a <see cref="RestClient"/> which is ready to send requests to the SeeShells API Server.
        /// To do so, it must make a request to a special endpoint on the SeeShells website to obtain the API's location. (see SeeShells Design Doc)
        /// </summary>
        /// <param name="url">URL of the SeeShells Website that contains the special endpoint.</param>
        /// <param name="endpoint">An endpoint which returns the URL of the API.</param>
        /// <returns>A Task containing a Rest Client ready to make requests to the API.</returns>
        public static async Task<IRestClient> GetAPIClientAsync(string url = DEFAULT_WEBSITE_URL, string endpoint = DEFAULT_WEBSITE_API_ENDPOINT)
        {
            //"cache" the location of the API, so dont do the network call again if we dont have to.
            if (apiClient == null) { 
                RestClient client = new RestClient(url);

                var request = new RestRequest(endpoint, DataFormat.None);
                //hit the website to get the URL of the API endpoint
                var response = await client.ExecuteGetAsync(request);
                if (response.IsSuccessful)
                {
                    try
                    {
                        Uri apiURL = new Uri(response.Content);

                        apiClient = new RestClient(apiURL);

                    } catch (UriFormatException ex)
                    {
                        logger.Error(ex);
                    }
                } 
                else //without the API endpoint we cant proceed 
                {
                    logger.Error("Couldn't retreive API Endpoint:\n" +
                        response.ErrorMessage + "\n" +
                        response.Content);
                }
            }

            return apiClient;
        }

    }
}
