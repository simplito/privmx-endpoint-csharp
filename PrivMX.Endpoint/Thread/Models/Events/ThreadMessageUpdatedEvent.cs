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

namespace PrivMX.Endpoint.Thread.Models.Events
{
    /// <summary>
    /// Holds data of event that arrives when Thread message is updated.
    /// </summary>
    public class ThreadMessageUpdatedEvent : Core.Models.Event
    {
        /// <summary>
        /// Event constructor
        /// </summary>
        public ThreadMessageUpdatedEvent() : base("threadUpdatedMessage")
        {
            
        }
        
        /// <summary>
        /// detailed information about Message
        /// </summary>
        public Message Data { get; set; } = null!;
    }
}
