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
    /// Represents payload of the StreamUpdatedEvent.
    /// </summary>
    public class StreamUpdatedEventData
    {
        /// <summary>
        /// ID of the streamRoom
        /// </summary>
        public string StreamRoomId { get; set; }
        
        /// <summary>
        /// List od added streams
        /// </summary>
        public List<StreamInfo> StreamsAdded { get; set; }
        
        /// <summary>
        /// List of removed streams
        /// </summary>
        public List<StreamInfo> StreamsRemoved { get; set; }
        
        /// <summary>
        /// List of modified stream tracks
        /// </summary>
        public List<StreamTrackModification> StreamsModified { get; set; }
    }
}