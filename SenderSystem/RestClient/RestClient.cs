using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;

namespace Digst.DigitalPost.SSLClient.Clients
{
    public class RestClient
    {
        private readonly HttpClient httpClient;

        private readonly ILogger logger;

        public RestClient(ILogger<RestClient> logger, HttpClient httpClient)
        {
            this.httpClient = httpClient;

            this.logger = logger;
        }

        public async Task<HttpResponseMessage> Get(string uriString, string apiKey, string accept = "application/json")
        {
            logger.LogDebug("Sending GET request. uri: {uri}", uriString);

            HttpRequestMessage request = CreateHttpRequestMessage(HttpMethod.Get, uriString, apiKey);
            if (accept != null)
            {
                request.Headers.Add("Accept", accept);
            }

            return await SendRequest(request);
        }

        private async Task<HttpResponseMessage> SendRequest(HttpRequestMessage request)
        {
            return await httpClient.SendAsync(request);
        }


        public async Task<HttpResponseMessage> Post(string uriString, HttpContent requestBody, string apiKey)
        {
            logger.LogDebug("Sending POST request. uri: {}", uriString);

            HttpRequestMessage request = CreateHttpRequestMessage(HttpMethod.Post, uriString, apiKey);
            request.Content = requestBody;

            return await SendRequest(request);
        }

        private HttpRequestMessage CreateHttpRequestMessage(HttpMethod method, string requestUri, string apiKey)
        {
            HttpRequestMessage request = new HttpRequestMessage(method, requestUri);
            if (apiKey != null)
            {
                request.Headers.Authorization = new AuthenticationHeaderValue("Basic", apiKey);
            }

            return request;
        }
    }
}