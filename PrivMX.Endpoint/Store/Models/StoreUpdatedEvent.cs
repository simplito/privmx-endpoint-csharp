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

namespace PrivMX.Endpoint.Store.Models
{
    /// <summary>
    /// Represents the event of type "storeUpdated".
    /// 
    /// This event is emitting when Store events are subscribed and a Store is updated.
    /// </summary>
    public class StoreUpdatedEvent : Core.Models.Event
    {
        /// <summary>
        /// Updated Store.
        /// </summary>
        public Store Data { get; set; }
    }
}
