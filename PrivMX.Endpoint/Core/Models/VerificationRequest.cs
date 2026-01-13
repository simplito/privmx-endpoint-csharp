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
    /// Request used when veryfying using an external service such as an application server or a PKI server.
    /// </summary>
    public class VerificationRequest
    {
        /// <summary>
        /// Id of the Context
        /// </summary>
        public string ContextId { get; set; }
        
        /// <summary>
        /// Id of the sender
        /// </summary>
        public string SenderId { get; set; }
        
        /// <summary>
        /// Public key of the sender
        /// </summary>
        public string SenderPubKey { get; set; }

        /// <summary>
        /// The data creation date
        /// </summary>
        public long Date { get; set; }
        
        /// <summary>
        /// Bridge Identity.
        /// </summary>
        public BridgeIdentity? BridgeIdentity { get; set; }
    }
}