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
    /// Contains information about the changed collection.
    /// </summary>
    public class CollectionChangedEventData
    {
        /// <summary>
        /// Type of the module
        /// </summary>
        public string ModuleType { get; set; } = null!;
        
        /// <summary>
        /// ID of the module
        /// </summary>
        public string ModuleId { get; set; } = null!;
        
        /// <summary>
        /// Count of affected items
        /// </summary>
        public long AffectedItemsCount { get; set; }
        
        /// <summary>
        /// List of item changes
        /// </summary>
        public List<CollectionItemChange> Items { get; set; } = null!;
    }
}