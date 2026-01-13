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

using System.Collections.Generic;

namespace PrivMX.Endpoint.Core.Models.Events
{
    /// <summary>
    /// Contains information about changed statuses of users in the Context.
    /// </summary>
    public class ContextUsersStatusChangedEventData
    {
        /// <summary>
        /// ID of the Context
        /// </summary>
        public string ContextId {get; set;} = null!;
        
        /// <summary>
        /// List of users with their changed statuses
        /// </summary>
        public List<UserWithAction> Users {get; set;} = null!;
    }
}