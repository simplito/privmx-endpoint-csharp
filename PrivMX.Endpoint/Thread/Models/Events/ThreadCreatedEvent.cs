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
    /// Holds data of event that arrives when Thread is created.
    /// </summary>
    public class ThreadCreatedEvent : Core.Models.Event
    {
        /// <summary>
        /// Event constructor
        /// </summary>
        public ThreadCreatedEvent() : base("threadCreated")
        {
            
        }
        
        /// <summary>
        /// Created Thread.
        /// </summary>
        public Thread Data { get; set; } = null!;
    }
}
