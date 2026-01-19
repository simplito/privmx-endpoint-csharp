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
    /// Represents payload of the StreamUnpublishedEvent.
    /// </summary>
    public class StreamUnpublishedEventData
    {
        /// <summary>
        /// ID of the streamRoom
        /// </summary>
        public string StreamRoomId { get; set; }
        
        /// <summary>
        /// ID of the stream
        /// </summary>
        public long StreamId { get; set; }
    }
}