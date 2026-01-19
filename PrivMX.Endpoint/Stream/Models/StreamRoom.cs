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

namespace PrivMX.Endpoint.Stream.Models
{
    /// <summary>
    /// Holds information about a stream room.
    /// </summary>
    public class StreamRoom
    {
        /// <summary>
        /// ID of the Context
        /// </summary>
        public string ContextId { get; set; }
        
        /// <summary>
        /// ID of the streamRoom
        /// </summary>
        public string StreamRoomId { get; set; }
        
        /// <summary>
        /// StreamRoom creation timestamp
        /// </summary>
        public long CreateDate { get; set; }
        
        /// <summary>
        /// ID of user who created the StreamRoom
        /// </summary>
        public string Creator { get; set; }
        
        /// <summary>
        /// StreamRoom last modification timestamp
        /// </summary>
        public long LastModificationDate { get; set; }
        
        /// <summary>
        /// ID of the user who last modified the StreamRoom
        /// </summary>
        public string LastModifier { get; set; }
        
        /// <summary>
        /// list of users (their IDs) with access to the StreamRoom
        /// </summary>
        public List<string> Users { get; set; }
        
        /// <summary>
        /// list of managers (their IDs) with access to the StreamRoom
        /// </summary>
        public List<string> Managers { get; set; }
        
        /// <summary>
        /// version number (changes on updates)
        /// </summary>
        public long Version { get; set; }
        
        /// <summary>
        /// StreamRoom's public metadata
        /// </summary>
        public byte[] PublicMeta { get; set; }
        
        /// <summary>
        /// StreamRoom's private metadata
        /// </summary>
        public byte[] PrivateMeta { get; set; }
        
        /// <summary>
        /// StreamRoom's policies
        /// </summary>
        public ContainerPolicy Policy { get; set; }
        
        /// <summary>
        /// Retrieval and decryption status code
        /// </summary>
        public long StatusCode { get; set; }
        
        /// <summary>
        /// Version of the StreamRoom data structure and how it is encoded/encrypted
        /// </summary>
        public long SchemaVersion { get; set; }
    }
}