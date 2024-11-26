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
    /// Represents payload of the InboxEntryDeletedEvent.
    /// </summary>
    public class InboxEntryDeletedEventData
    {
        /// <summary>
        /// ID of the Inbox that the entry is deleted from.
        /// </summary>
        public string InboxId { get; set; }

        /// <summary>
        /// ID of the deleted Inbox entry.
        /// </summary>
        public string EntryId { get; set; }
    }
}
