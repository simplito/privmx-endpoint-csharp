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

namespace PrivMX.Endpoint.Core
{
    public interface IBackendRequester
    {
        string BackendRequest(string serverUrl, byte[] accessToken, string method, string paramsAsJson);
        string BackendRequest(string serverUrl, string method, string paramsAsJson);
        string BackendRequest(string serverUrl, string apiKeyId, byte[] apiKeySecret, long mode, string method, string paramsAsJson);
    }
}
