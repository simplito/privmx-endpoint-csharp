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
    /// Holds StreamRoom model
    /// </summary>
    public class RoomModel
    {
        /// <summary>
        /// Id of a StreamRoom
        /// </summary>
        public string RoomId { get; set; }

        public RoomModel(string roomId)
        {
            RoomId = roomId;
        }
    }
}