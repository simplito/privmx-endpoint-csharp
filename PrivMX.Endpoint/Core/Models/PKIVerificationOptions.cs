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

namespace PrivMX.Endpoint.Core.Models
{
    /// <summary>
    /// PrivMX Bridge server instance verification options using a PKI server.
    /// </summary>
    public class PKIVerificationOptions
    {
        /// <summary>
        /// Bridge public Key.
        /// </summary>
        public string? BridgePubKey { get; set; }
        
        /// <summary>
        /// Bridge instance Id given by PKI.
        /// </summary>
        public string? BridgeInstanceId { get; set; }
    }
}