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

namespace PrivMX.Endpoint.Stream.Models.StreamApiLow
{
    /// <summary>
    /// Holds information about new streams
    /// </summary>
    public class NewStreams
    {
        /// <summary>
        /// Stream room
        /// </summary>
        public string Room { get; set; }
        
        /// <summary>
        /// List of streamInfos
        /// </summary>
        public List<StreamInfo> Streams { get; set; }
    }
}