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
    /// Holds data of event that arrives after StreamJoin - contains information about updates on publishers streams
    /// one can subscribe to.
    /// </summary>
    public class StreamsUpdatedEvent : Core.Models.Event
    {
        /// <summary>
        /// Event constructor
        /// </summary>
        public StreamsUpdatedEvent() : base("streamsUpdated")
        {
            
        }
        
        /// <summary>
        /// Event data
        /// </summary>
        public StreamsUpdatedData Data { get; set; }
    }
}