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
    /// Represents payload of the ThreadDeletedMessageEvent.
    /// </summary>
    public class ThreadDeletedMessageEventData
    {
        /// <summary>
        /// ID of the Thread that the message is deleted from.
        /// </summary>
        public string ThreadId { get; set; } = null!;

        /// <summary>
        /// ID of the deleted message.
        /// </summary>
        public string MessageId { get; set; } = null!;
    }
}
