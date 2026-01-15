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
    /// Holds Kvdb statistical data.
    /// </summary>
    public class KvdbStatsEventData
    {
        /// <summary>
        /// Kvdb ID
        /// </summary>
        public string KvdbId { get; set; }
        
        /// <summary>
        /// timestamp of the most recent Kvdb item
        /// </summary>
        public long LastEntryDate { get; set; }
        
        /// <summary>
        /// updated number of entries in the Kvdb
        /// </summary>
        public long Entries { get; set; }
    }
}