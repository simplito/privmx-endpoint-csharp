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

namespace PrivMX.Endpoint.Stream.Models.Events
{
    /// <summary>
    /// Holds data of event that arrives when stream is joined.
    /// </summary>
    public class StreamJoinedEvent : Core.Models.Event
    {
        /// <summary>
        /// Event constructor
        /// </summary>
        public StreamJoinedEvent() : base("streamJoined")
        {
            
        }
        
        /// <summary>
        /// Event data
        /// </summary>
        public StreamEventData Data { get; set; }
    }
}