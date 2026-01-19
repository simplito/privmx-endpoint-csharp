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
    /// Holds information Session Id Model
    /// </summary>
    public class UpdateSessionIdModel
    {
        /// <summary>
        /// ID of the StreamRoom
        /// </summary>
        public string StreamRoomId { get; set; }
        
        /// <summary>
        /// Type of the connection
        /// </summary>
        public string ConnectionType { get; set; }
        
        /// <summary>
        /// ID of the Session
        /// </summary>
        public long SessionId { get; set; }
    }
}