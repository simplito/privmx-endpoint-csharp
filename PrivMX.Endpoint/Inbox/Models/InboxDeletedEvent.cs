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

using PrivMX.Endpoint.Core.Models;

namespace PrivMX.Endpoint.Inbox.Models
{
    /// <summary>
    /// Represents the event of type "inboxDeleted".
    /// This event is emitting when Inbox events are subscribed and a Inbox is deleted.
    /// </summary>
    public class InboxDeletedEvent : Event
    {
        /// <summary>
        /// Metadata of the deleted Inbox.
        /// </summary>
        public InboxDeletedEventData Data { get; set; }
    }
}
