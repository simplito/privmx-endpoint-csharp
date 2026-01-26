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
    /// Holds information about sdp model with type
    /// </summary>
    public class SdpWithTypeModel
    {
        /// <summary>
        /// ID of the Room
        /// </summary>
        public string RoomId { get; set; }
        
        /// <summary>
        /// Session description protocol
        /// </summary>
        public string Sdp { get; set; }
        
        /// <summary>
        /// Sdp type
        /// </summary>
        public string Type { get; set; }
    }
}