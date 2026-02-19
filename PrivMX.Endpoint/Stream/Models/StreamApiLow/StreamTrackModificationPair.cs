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

namespace PrivMX.Endpoint.Stream.Models.StreamApiLow
{
    /// <summary>
    /// Holds information about a pair of streamTracks, for modification purposes
    /// </summary>
    public class StreamTrackModificationPair
    {
        /// <summary>
        /// StreamTrack before modification
        /// </summary>
        public StreamTrackInfo? Before { get; set; }
        
        /// <summary>
        /// StreamTrack after modification
        /// </summary>
        public StreamTrackInfo? After { get; set; }

        /// <summary>
        /// StreamTrackModificationPair constructor
        /// </summary>
        public StreamTrackModificationPair()
        {
            
        }

        /// <summary>
        /// StreamTrackModificationPair constructor
        /// </summary>
        /// <param name="before">StreamTrack before modification</param>
        /// <param name="after">StreamTrack after modification</param>
        public StreamTrackModificationPair(StreamTrackInfo? before, StreamTrackInfo? after)
        {
            Before = before;
            After = after;
        }
    }
}