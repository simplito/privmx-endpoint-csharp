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

namespace PrivMX.Endpoint.Inbox.Models
{
    /// <summary>
    /// Represents public data of a Inbox.
    /// </summary>
    public class InboxPublicView
    {
        /// <summary>
        /// ID of the Inbox.
        /// </summary>
        public string InboxId { get; set; } = null!;

        /// <summary>
        /// Number of the Inbox updates.
        /// </summary>
        public long Version { get; set; }

        /// <summary>
        /// Public metadata.
        /// </summary>
        public byte[] PublicMeta { get; set; } = null!;
    }
}
