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

namespace PrivMX.Endpoint.Thread.Models
{
    /// <summary>
    /// Represents server metadata.
    /// </summary>
    public class ServerMessageInfo
    {
        /// <summary>
        /// ID of the Thread.
        /// </summary>
        public string ThreadId { get; set; }

        /// <summary>
        /// ID of the message.
        /// </summary>
        public string MessageId { get; set; }

        /// <summary>
        /// Server creation timestamp.
        /// </summary>
        public long CreateDate { get; set; }

        /// <summary>
        /// ID of the creator user.
        /// </summary>
        public string Author { get; set; }
    }
}
