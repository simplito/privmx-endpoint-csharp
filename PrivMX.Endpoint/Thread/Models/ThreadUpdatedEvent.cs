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
    /// Represents the event of type "threadUpdated".
    /// 
    /// This event is emitted when Thread events are subscribed and a Thread is updated.
    /// </summary>
    public class ThreadUpdatedEvent : Event
    {
        /// <summary>
        /// Updated Thread.
        /// </summary>
        public Thread Data { get; set; } = null!;
    }
}
