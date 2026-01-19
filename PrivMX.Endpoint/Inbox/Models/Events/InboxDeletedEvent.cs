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
    /// Holds data of event that arrives when Inbox is deleted.
    /// </summary>
    public class InboxDeletedEvent : Core.Models.Event
    {
        /// <summary>
        /// Event constructor
        /// </summary>
        public InboxDeletedEvent() : base("inboxDeleted")
        {
            
        }
        
        /// <summary>
        /// Metadata of the deleted Inbox.
        /// </summary>
        public InboxDeletedEventData Data { get; set; } = null!;
    }
}
