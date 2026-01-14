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
using PrivMX.Endpoint.Core.Models;

namespace PrivMX.Endpoint.Kvdb.Models
{
    /// <summary>
    /// Holds all available information about a Kvdb.
    /// </summary>
    public class Kvdb
    {
        /// <summary>
        /// ID of the Context
        /// </summary>
        public string ContextId { get; set; }
        
        /// <summary>
        /// ID of the Context
        /// </summary>
        public string KvdbId { get; set; }
        
        /// <summary>
        /// Kvdb creation timestamp
        /// </summary>
        public long CreateDate { get; set; }
        
        /// <summary>
        /// ID of user who created the Kvdb
        /// </summary>
        public string Creator { get; set; }
        
        /// <summary>
        /// Kvdb last modification timestamp
        /// </summary>
        public long LastModificationDate { get; set; }
        
        /// <summary>
        /// ID of the user who last modified the Kvdb
        /// </summary>
        public string LastModifier { get; set; }
        
        /// <summary>
        /// list of users (their IDs) with access to the Kvdb
        /// </summary>
        public List<string> Users { get; set; }
        
        /// <summary>
        /// list of users (their IDs) with management rights
        /// </summary>
        public List<string> Managers { get; set; }
        
        /// <summary>
        /// version number (changes on updates)
        /// </summary>
        public long Version { get; set; }
        
        /// <summary>
        /// Kvdb's public metadata
        /// </summary>
        public byte[] PublicMeta { get; set; }
        
        /// <summary>
        /// Kvdb's private metadata
        /// </summary>
        public byte[] PrivateMeta { get; set; }
        
        /// <summary>
        /// total number of entries in the Kvdb
        /// </summary>
        public long Entries { get; set; }
        
        /// <summary>
        /// Timestamp of the last added entry
        /// </summary>
        public long LastEntrydata { get; set; }
        
        /// <summary>
        /// Kvdb's policies
        /// </summary>
        public ContainerPolicy Policy { get; set; }
        
        /// <summary>
        /// Retrieval and decryption status code
        /// </summary>
        public long StatusCode { get; set; }
        
        /// <summary>
        /// Version of the Kvdb data structure and how it is encoded/encrypted
        /// </summary>
        public long SchemaVersion { get; set; }
    }
}