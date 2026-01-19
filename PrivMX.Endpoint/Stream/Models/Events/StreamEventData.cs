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

using System.Collections.Generic;

namespace PrivMX.Endpoint.Stream.Models.Events
{
    /// <summary>
    /// Represents payload of the StreamEvent.
    /// </summary>
    public class StreamEventData
    {
        /// <summary>
        /// ID of the streamRoom
        /// </summary>
        public string StreamRoomId { get; set; }
        
        /// <summary>
        /// Stream IDs
        /// </summary>
        public List<long> StreamIds { get; set; }
        
        /// <summary>
        /// ID of modifier user
        /// </summary>
        public string UserId { get; set; }
    }
}