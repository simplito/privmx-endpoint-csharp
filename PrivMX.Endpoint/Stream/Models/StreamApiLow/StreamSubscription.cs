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
    /// Holds information about stream subscription
    /// </summary>
    public class StreamSubscription
    {
        /// <summary>
        /// ID of the Stream
        /// </summary>
        public long StreamId { get; set; }
        
        /// <summary>
        /// (optional) ID od the stream track
        /// </summary>
        public string? StreamTrackId { get; set; }
    }
}