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
    /// Represents the event of type "inboxCreated".
    /// 
    /// This event is emitting when Inbox events are subscribed and a new Inbox is created.
    /// </summary>
    public class InboxCreatedEvent : Event
    {
        /// <summary>
        /// Created Inbox.
        /// </summary>
        public Inbox Data { get; set; }
    }
}
