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
    /// Represents the event of type "inboxEntryCreated".
    /// 
    /// This event is emitted when Inbox entry events are subscribed and a new entry is created.
    /// </summary>
    public class InboxEntryCreatedEvent : Event
    {
        /// <summary>
        /// Created Inbox entry.
        /// </summary>
        public InboxEntry Data { get; set; } = null!;
    }
}
