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

using PrivMX.Endpoint.Core.Models;
using PrivMX.Endpoint.Thread.Models;

namespace PrivMX.Endpoint.Store.Models
{
    /// <summary>
    /// Represents the event of type "storeDeleted".
    /// 
    /// This event is emitted when Store events are subscribed and a Store is deleted.
    /// </summary>
    public class StoreDeletedEvent : Event
    {
        /// <summary>
        /// Metadata of the deleted Store.
        /// </summary>
        public StoreDeletedEventData Data { get; set; } = null!;
    }
}
