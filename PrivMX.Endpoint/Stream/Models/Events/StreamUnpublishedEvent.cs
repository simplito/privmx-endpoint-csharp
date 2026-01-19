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
    /// Holds data of event that arrives when Stream didn't get published.
    /// </summary>
    public class StreamUnpublishedEvent : Core.Models.Event
    {
        /// <summary>
        /// Event constructor
        /// </summary>
        public StreamUnpublishedEvent() : base("streamUnpublished")
        {
            
        }
        
        /// <summary>
        /// Event data
        /// </summary>
        public StreamUnpublishedEventData Data { get; set; }
    }
}