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
    /// Represents a user in a Context.
    /// </summary>
    public class UserInfo
    {
        /// <summary>
        /// User ID and and its associated public key in the Context.
        /// </summary>
        public UserWithPubKey User { get; set; } = null!;

        /// <summary>
        /// Status that idicates whether the user is connected to Bridge.
        /// </summary>
        public bool IsActive { get; set; }
        
        /// <summary>
        /// User last status change or no value if they have never logged in
        /// </summary>
        UserStatusChange? LastStatusChange { get; set; }
    }
}
