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
    /// Contains information about the change of user status.
    /// </summary>
    public class UserStatusChange
    {
        /// <summary>
        /// User status change action, which can be "login" or "logout"
        /// </summary>
        public string Action { get; set; }
        
        /// <summary>
        /// Timestamp of the change
        /// </summary>
        public long Timestamp { get; set; }
    }
}