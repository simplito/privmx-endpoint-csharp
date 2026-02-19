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
    /// Holds all available information about a Stream
    /// </summary>
    public class Stream
    {
        /// <summary>
        /// ID of the Stream
        /// </summary>
        public long StreamId { get; set; }
        
        /// <summary>
        /// ID of the user who created the Stream
        /// </summary>
        public string UserId { get; set; }

        /// <summary>
        /// Stream constructor
        /// </summary>
        public Stream()
        {
            
        }
        
        /// <summary>
        /// Stream constructor
        /// </summary>
        /// <param name="streamId">ID of the Stream</param>
        /// <param name="userId">ID of the user who created the Stream</param>
        public Stream(long streamId, string userId)
        {
            StreamId = streamId;
            UserId = userId;
        }
    }
}