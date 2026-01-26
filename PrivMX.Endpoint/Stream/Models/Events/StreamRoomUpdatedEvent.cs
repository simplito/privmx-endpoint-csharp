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

using PrivMX.Endpoint.Stream.Models.StreamApiLow;

namespace PrivMX.Endpoint.Stream.Models.Events
{
    /// <summary>
    /// Holds data of event that arrives when StreamRoom is updated.
    /// </summary>
    public class StreamRoomUpdatedEvent : Core.Models.Event
    {
        /// <summary>
        /// Event constructor
        /// </summary>
        public StreamRoomUpdatedEvent() : base("streamRoomUpdated")
        {
            
        }
        
        /// <summary>
        /// All available StreamRoom information
        /// </summary>
        public StreamRoom Data { get; set; }
    }
}