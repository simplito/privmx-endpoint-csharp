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

namespace PrivMX.Endpoint.Kvdb.Models.Events
{
    /// <summary>
    /// Holds data of event that arrives when Kvdb stats change.
    /// </summary>
    public class KvdbStatsChangedEvent : Core.Models.Event
    {
        /// <summary>
        /// Event constructor
        /// </summary>
        public KvdbStatsChangedEvent() : base("kvdbStatsChanged")
        {
        }
        
        /// <summary>
        /// event data
        /// </summary>
        public KvdbStatsEventData Data { get; set; }
    }
}