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
    /// Holds information about stream publish result
    /// </summary>
    public class StreamPublishResult
    {
        /// <summary>
        /// Marks if the stream was published succesfully
        /// </summary>
        public bool Published { get; set; }
        
        /// <summary>
        /// (optional) Published stream data
        /// </summary>
        public PublishedStreamData? Data { get; set; }
    }
}