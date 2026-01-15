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
    /// Holds all available information about a Entry.
    /// </summary>
    public class KvdbEntry
    {
        /// <summary>
        /// Entry information created by server
        /// </summary>
        public ServerKvdbEntryInfo Info { get; set; }
        
        /// <summary>
        /// Entry public metadata
        /// </summary>
        public byte[] PublicData { get; set; }
        
        /// <summary>
        /// Entry private metadata
        /// </summary>
        public byte[] PrivateData { get; set; }
        
        /// <summary>
        /// Entry data
        /// </summary>
        public byte[] Data { get; set; }
        
        /// <summary>
        /// Public key of an author of the entry
        /// </summary>
        public string AuthorPubKey { get; set; }
        
        /// <summary>
        /// Version number (changes on every on existing item)
        /// </summary>
        public long Version { get; set; }
        
        /// <summary>
        /// Retrieval and decryption status code
        /// </summary>
        public long StatusCode { get; set; }
        
        /// <summary>
        /// Version of the Entry data structure and how it is encoded/encrypted
        /// </summary>
        public long SchemaVersion { get; set; }
    }
}