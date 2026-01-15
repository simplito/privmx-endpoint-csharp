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
    /// Holds information of `KvdbDeletedEntryEvent`.
    /// </summary>
    public class KvdbDeletedEntryEventData
    {
        /// <summary>
        /// Kvdb ID
        /// </summary>
        public string KvdbId { get; set; }
        
        /// <summary>
        /// Key of deleted Entry
        /// </summary>
        public string kvdbEntryKey { get; set; }
    }
}