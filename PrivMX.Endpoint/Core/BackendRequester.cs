//
// PrivMX Endpoint C#
// Copyright © 2024 Simplito sp. z o.o.
//
// This file is part of the PrivMX Platform (https://privmx.dev).
// This software is Licensed under the MIT License.
//
// See the License for the specific language governing permissions and
// limitations under the License.
//

using PrivMX.Endpoint.Core.Internal;
using System;
using System.Collections.Generic;

namespace PrivMX.Endpoint.Core
{
    public class BackendRequester : IBackendRequester
    {
        public readonly IntPtr ptr;
        private readonly Executor executor = new Executor(new BackendRequesterNative());

        /// <summary>
        /// Creates an instance of the <see cref="BackendRequester"/>.
        /// </summary>
        /// <returns>Created instance of the <see cref="BackendRequester"/>.</returns>
        static public BackendRequester Create()
        {
            BackendRequester backendRequester = new BackendRequester();
            return backendRequester;
        }

        private BackendRequester()
        {
            BackendRequesterNative.privmx_endpoint_newBackendRequester(out ptr);
        }

        ~BackendRequester()
        {
            BackendRequesterNative.privmx_endpoint_freeBackendRequester(ptr);
        }

        /// <summary>
        /// Sends a request to PrivMX Bridge API using access token for authorization.
        /// </summary>
        /// <param name="serverUrl">PrivMX Bridge server URL.</param>
        /// <param name="accessToken">Token for authorization (see PrivMX Bridge API for more details).</param>
        /// <param name="method">API method to call.</param>
        /// <param name="paramsAsJson">API method's parameters in JSON format.</param>
        /// <returns>JSON string representing raw server response.</returns>
        public string BackendRequest(string serverUrl, string accessToken, string method, string paramsAsJson)
        {
            return executor.Execute<string>(ptr, (int)BackendRequesterNative.Method.BackendRequest, new List<object?> { serverUrl, accessToken, method, paramsAsJson });
        }

        /// <summary>
        /// Sends request to PrivMX Bridge API.
        /// </summary>
        /// <param name="serverUrl">PrivMX Bridge server URL.</param>
        /// <param name="method">API method to call.</param>
        /// <param name="paramsAsJson">API method's parameters in JSON format.</param>
        /// <returns>JSON string representing raw server response.</returns>
        public string BackendRequest(string serverUrl, string method, string paramsAsJson)
        {
            return executor.Execute<string>(ptr, (int)BackendRequesterNative.Method.BackendRequest, new List<object?> { serverUrl, method, paramsAsJson });
        }

        /// <summary>
        /// Sends a request to PrivMX Bridge API using pair of API KEY ID and API KEY SECRET for authorization.
        /// </summary>
        /// <param name="serverUrl">PrivMX Bridge server URL.</param>
        /// <param name="apiKeyId">API KEY ID (see PrivMX Bridge API for more details).</param>
        /// <param name="apiKeySecret">API KEY SECRET (see PrivMX Bridge API for more details).</param>
        /// <param name="mode">Allows you to set whether the request should be signed (mode = 1) or plain (mode = 0).</param>
        /// <param name="method">API method to call.</param>
        /// <param name="paramsAsJson">API method's parameters in JSON format.</param>
        /// <returns>JSON string representing raw server response.</returns>
        public string BackendRequest(string serverUrl, string apiKeyId, string apiKeySecret, long mode, string method, string paramsAsJson)
        {
            return executor.Execute<string>(ptr, (int)BackendRequesterNative.Method.BackendRequest, new List<object?> { serverUrl, apiKeyId, apiKeySecret, mode, method, paramsAsJson });
        }
    }
}
