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
    /// Represents the event of type "threadCreated".
    /// 
    /// This event is emitted when Thread events are subscribed and a new Thread is created.
    /// </summary>
    public class ThreadCreatedEvent : Event
    {
        /// <summary>
        /// Created Thread.
        /// </summary>
        public Thread Data { get; set; }
    }
}
