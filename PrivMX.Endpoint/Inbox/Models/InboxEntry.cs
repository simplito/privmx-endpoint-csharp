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

using PrivMX.Endpoint.Store.Models;
using System.Collections.Generic;

namespace PrivMX.Endpoint.Inbox.Models
{
    /// <summary>
    /// Reprezents an entry in the Inbox.
    /// </summary>
    public class InboxEntry
    {
        /// <summary>
        /// ID of the entry in the Inbox.
        /// </summary>
        public string EntryId { get; set; } = null!;

        /// <summary>
        /// ID of the Inbox.
        /// </summary>
        public string InboxId { get; set; } = null!;

        /// <summary>
        /// Payload of the entry.
        /// </summary>
        public byte[] Data { get; set; } = null!;

        /// <summary>
        /// List of files in the entry.
        /// </summary>
        public List<File> Files { get; set; } = null!;

        /// <summary>
        /// Verified public key of the Data author.
        /// </summary>
        public string AuthorPubKey { get; set; } = null!;

        /// <summary>
        /// Server creation timestamp.
        /// </summary>
        public long CreateDate { get; set; }

        /// <summary>
        /// Status code of decryption and verification of the message.
        /// 
        /// If value is equal 0, then the message is successfully decrypted and verified. Otherwise, status code is compatible with codes of exceptions.
        /// </summary>
        public long StatusCode { get; set; }
    }
}
