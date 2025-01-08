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

namespace PrivMX.Endpoint.Inbox.Models
{
    /// <summary>
    /// Represents an Inbox.
    /// </summary>
    public class Inbox
    {
        /// <summary>
        /// ID of the Inbox.
        /// </summary>
        public string InboxId { get; set; }

        /// <summary>
        /// ID og the Context.
        /// </summary>
        public string ContextId { get; set; }

        /// <summary>
        /// Server creation timestamp.
        /// </summary>
        public long CreateDate { get; set; }

        /// <summary>
        /// ID of the creator user.
        /// </summary>
        public string Creator { get; set; }

        /// <summary>
        /// Last modification timestamp.
        /// </summary>
        public long LastModificationDate { get; set; }

        /// <summary>
        /// ID of the user who was last a modifier.
        /// </summary>
        public string LastModifier { get; set; }

        /// <summary>
        /// List of user IDs that have access to the Inbox.
        /// </summary>
        public List<string> Users { get; set; }

        /// <summary>
        /// List of user IDs that have management rights to the Inbox.
        /// </summary>
        public List<string> Managers { get; set; }

        /// <summary>
        /// Number of the Inbox updates.
        /// </summary>
        public long Version { get; set; }

        /// <summary>
        /// Public metadata.
        /// </summary>
        public byte[] PublicMeta { get; set; }

        /// <summary>
        /// Private metadata.
        /// </summary>
        public byte[] PrivateMeta { get; set; }

        /// <summary>
        /// Files configuration for the Inbox. 
        /// </summary>
        public FilesConfig FilesConfig { get; set; }

        /// <summary>
        /// Inbox policy.
        /// </summary>
        public ContainerPolicyWithoutItem Policy { get; set; }

        /// <summary>
        /// Status code of decryption and verification of the Thread.
        /// If value is equal 0, then the Thread is successfully decrypted and verified. Otherwise, status code is compatible with codes of exceptions.
        /// </summary>
        public long StatusCode { get; set; }
        
    }
}
