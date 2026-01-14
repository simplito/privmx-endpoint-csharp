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

namespace PrivMX.Endpoint.Kvdb.Models
{
    /// <summary>
    /// Holds Kvdb entry's information created by the server
    /// </summary>
    public class ServerKvdbEntryInfo
    {
        /// <summary>
        /// ID of the Kvdb
        /// </summary>
        public string KvdbId { get; set; }
        
        /// <summary>
        /// Kvdb entry's key
        /// </summary>
        public string Key { get; set; }
        
        /// <summary>
        /// Entry's creation timestamp
        /// </summary>
        public long CreateDate { get; set; }
        
        /// <summary>
        /// ID of the user who created the entry
        /// </summary>
        public string Author { get; set; }
    }
}