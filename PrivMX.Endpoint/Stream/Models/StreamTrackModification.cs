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

namespace PrivMX.Endpoint.Stream.Models
{
    /// <summary>
    /// Holds streamTrack modification data
    /// </summary>
    public class StreamTrackModification
    {
        /// <summary>
        /// ID of the stream to modify
        /// </summary>
        public long StreamId { get; set; }
        
        /// <summary>
        /// List od streamTrackModificationPairs 
        /// </summary>
        public List<StreamTrackModificationPair> Tracks { get; set; }
    }
}