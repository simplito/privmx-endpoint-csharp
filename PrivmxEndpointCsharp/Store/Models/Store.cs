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

namespace PrivMX.Endpoint.Store.Models
{
    /// <summary>
    /// Represents the Store.
    /// </summary>
    public class Store
    {
        /// <summary>
        /// ID of the Context.
        /// </summary>
        public string ContextId { get; set; }

        /// <summary>
        /// ID of the Store.
        /// </summary>
        public string StoreId { get; set; }

        /// <summary>
        /// Server creation timestamp.
        /// </summary>
        public long CreateDate {get; set; }

        /// <summary>
        /// ID of the creator user.
        /// </summary>
        public string Creator { get; set; }

        /// <summary>
        /// Last modification timestamp.
        /// </summary>
        public long LastModificationDate {get; set; }

        /// <summary>
        /// ID of the user who was last a modifier.
        /// </summary>
        public string LastModifier { get; set; }

        /// <summary>
        /// List of user IDs that have access to the Store.
        /// </summary>
        public List<string> Users { get; set; }

        /// <summary>
        /// List of user IDs that have management rights to the Store.
        /// </summary>
        public List<string> Managers { get; set; }

        /// <summary>
        /// Number of the Store updates.
        /// </summary>
        public long Version { get; set; }

        /// <summary>
        /// Timestamp of the last file in the Store, or the Store creation timestamp if no files in.
        /// </summary>
        public long LastFileDate {get; set; }

        /// <summary>
        /// Public metadata.
        /// </summary>
        public byte[] PublicMeta { get; set; }

        /// <summary>
        /// Private metadata.
        /// </summary>
        public byte[] PrivateMeta { get; set; }

        /// <summary>
        /// Number of files in the Store.
        /// </summary>
        public long FilesCount { get; set; }

        /// <summary>
        /// Status code of decryption and verification of the Thread.
        /// If value is equal 0, then the Thread is successfully decrypted and verified. Otherwise, status code is compatible with codes of exceptions.
        /// </summary>
        public long StatusCode { get; set; }
    }
}
