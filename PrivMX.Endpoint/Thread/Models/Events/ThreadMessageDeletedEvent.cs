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
    /// Holds data of event that arrives when Thread message is deleted.
    /// </summary>
    public class ThreadMessageDeletedEvent : Core.Models.Event
    {
        /// <summary>
        /// Event constructor
        /// </summary>
        public ThreadMessageDeletedEvent() : base("threadMessageDeleted")
        {
            
        }
        
        /// <summary>
        /// Event data
        /// </summary>
        public ThreadDeletedMessageEventData Data { get; set; } = null!;
    }
}
