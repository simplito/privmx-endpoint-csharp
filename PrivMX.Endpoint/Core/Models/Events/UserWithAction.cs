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

namespace PrivMX.Endpoint.Core.Models.Events
{
    /// <summary>
    /// Contains the user with their status change action
    /// </summary>
    public class UserWithAction
    {
        /// <summary>
        /// User
        /// </summary>
        public UserWithPubKey User {get; set;} = null!;
        
        /// <summary>
        /// User status change action, which can be "login" or "logout"
        /// </summary>
        public string Action { get; set; } = null!;
    }
}