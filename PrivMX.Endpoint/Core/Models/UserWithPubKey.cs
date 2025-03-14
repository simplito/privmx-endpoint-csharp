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
    /// Represents a user and its associated public key in a Context.
    /// </summary>
    public class UserWithPubKey
    {
        /// <summary>
        /// ID of the user in the Context.
        /// </summary>
        public string UserId { get; set; }

        /// <summary>
        /// Public key associated with the user.
        /// </summary>
        public string PubKey { get; set; }
    }
}
