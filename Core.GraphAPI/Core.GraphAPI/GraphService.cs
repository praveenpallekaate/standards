using Microsoft.Graph;
using Microsoft.Identity.Web;
using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace Core.GraphAPI
{
    /// <summary>
    /// Service creates graph client object
    /// </summary>
    public class GraphService
    {
        private readonly ITokenAcquisition _tokenAcquisition;
        private readonly string _endpoint;
        private readonly string[] _scopes;

        /// <summary>
        /// ctor
        /// </summary>
        /// <param name="tokenAcquisition">Tokenaquisition object</param>
        /// <param name="scopes">API scopes</param>
        /// <param name="endpoint">API url</param>
        public GraphService(ITokenAcquisition tokenAcquisition, string[] scopes, string endpoint)
        {
            _tokenAcquisition = tokenAcquisition;
            _scopes = scopes;
            _endpoint = endpoint;
        }

        /// <summary>
        /// Creates graph client
        /// </summary>
        /// <returns>Graph service client</returns>
        public GraphServiceClient GetGraphServiceClient()
        {
            return GetAuthenticatedGraphClient(async () =>
                    {
                        return await _tokenAcquisition.GetAccessTokenForUserAsync(_scopes);
                    }, 
                    _endpoint
                );
        }

        /// <summary>
        /// Graph client object that is authenticated
        /// </summary>
        /// <param name="acquireAccessToken">Access token</param>
        /// <param name="baseUrl">Endpoint url</param>
        /// <returns>Graph service client</returns>
        private static GraphServiceClient GetAuthenticatedGraphClient(Func<Task<string>> acquireAccessToken, string baseUrl)
        {
            return new GraphServiceClient(baseUrl, new CustomAuthenticationProvider(acquireAccessToken));
        }

        /// <summary>
        /// Used only for devops
        /// </summary>
        /// <param name="args"></param>
        static void Main(string[] args) { }
    }

    /// <summary>
    /// Custom auth provider
    /// </summary>
    class CustomAuthenticationProvider : IAuthenticationProvider
    {
        /// <summary>
        /// ctor
        /// </summary>
        /// <param name="acquireTokenCallback"></param>
        public CustomAuthenticationProvider(Func<Task<string>> acquireTokenCallback)
        {
            acquireAccessToken = acquireTokenCallback;
        }

        private readonly Func<Task<string>> acquireAccessToken;

        /// <summary>
        /// Authenticate request with token
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public async Task AuthenticateRequestAsync(HttpRequestMessage request)
        {
            string accessToken = await acquireAccessToken.Invoke();

            // Append the access token to the request.
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);
        }
    }
}
