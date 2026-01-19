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

namespace PrivMX.Endpoint.Stream.Models
{
    /// <summary>
    /// Holds published stream data
    /// </summary>
    public class PublishedStreamData
    {
        /// <summary>
        /// ID of a streamRoom
        /// </summary>
        public string StreamRoom { get; set; }
        
        /// <summary>
        /// Information about the stream
        /// </summary>
        public StreamInfo Stream { get; set; }
        
        /// <summary>
        /// ID of the user
        /// </summary>
        public string UserId { get; set; }
    }
}