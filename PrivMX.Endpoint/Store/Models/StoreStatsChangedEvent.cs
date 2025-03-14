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

namespace PrivMX.Endpoint.Thread.Models
{
    /// <summary>
    /// Represents the event of type "storeStatsChanged".
    /// 
    /// This event is emitting when Store events are subscribed and statistics of a Store is changed.
    /// </summary>
    public class StoreStatsChangedEvent : Event
    {
        /// <summary>
        /// Store statistics.
        /// </summary>
        public StoreStatsChangedEventData Data { get; set; }
    }
}
