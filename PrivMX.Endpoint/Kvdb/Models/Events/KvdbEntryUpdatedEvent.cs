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
    /// Holds data of event that arrives when Kvdb message is updated.
    /// </summary>
    public class KvdbEntryUpdatedEvent : Core.Models.Event
    {
        /// <summary>
        /// Event constructor
        /// </summary>
        public KvdbEntryUpdatedEvent() : base("kvdbEntryUpdated")
        {
        }
        
        /// <summary>
        /// detailed information about KvdbEntry
        /// </summary>
        public KvdbEntry Data { get; set; }
    }
}