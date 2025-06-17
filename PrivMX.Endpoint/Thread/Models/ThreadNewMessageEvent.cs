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

namespace PrivMX.Endpoint.Thread.Models
{
    /// <summary>
    /// Represents the event of type "threadNewMessage".
    /// 
    /// This event is emitting when message events are subscribed and a new message is created.
    /// </summary>
    public class ThreadNewMessageEvent : Core.Models.Event
    {
        /// <summary>
        /// Created message.
        /// </summary>
        public Message Data { get; set; }
    }
}
