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
    /// Holds stream's updated data
    /// </summary>
    public class StreamsUpdatedData
    {
        /// <summary>
        /// ID of stream room
        /// </summary>
        public string Room { get; set; }
        
        /// <summary>
        /// List of updated streams
        /// </summary>
        public List<UpdatedStreamData> Streams { get; set; }
    }
}