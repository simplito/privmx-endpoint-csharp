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

namespace PrivMX.Endpoint.Inbox.Models.Events
{
    /// <summary>
    /// Holds data of event that arrives when Inbox entry is created.
    /// </summary>
    public class InboxEntryCreatedEvent : Core.Models.Event
    {
        /// <summary>
        /// Event constructor
        /// </summary>
        public InboxEntryCreatedEvent() : base("inboxEntryCreated")
        {
            
        }
        
        /// <summary>
        /// Created Inbox entry.
        /// </summary>
        public InboxEntry Data { get; set; } = null!;
    }
}
