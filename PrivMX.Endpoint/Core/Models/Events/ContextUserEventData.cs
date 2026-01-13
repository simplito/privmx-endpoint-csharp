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
    /// Contains information about the user of the Context
    /// </summary>
    public class ContextUserEventData
    {
        /// <summary>
        /// ID of the Context
        /// </summary>
        public string ContextId { get; set; } = null!;
        
        /// <summary>
        /// User
        /// </summary>
        public UserWithPubKey User { get; set; } = null!;
    }
}