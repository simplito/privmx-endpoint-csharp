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
    /// Holds data of event that arrives when Stream is left.
    /// </summary>
    public class StreamLeftEvent : Core.Models.Event
    {
        /// <summary>
        /// Event constructor
        /// </summary>
        public StreamLeftEvent() : base("streamLeft")
        {
            
        }
        
        /// <summary>
        /// Event data
        /// </summary>
        public StreamEventData Data { get; set; }
    }
}