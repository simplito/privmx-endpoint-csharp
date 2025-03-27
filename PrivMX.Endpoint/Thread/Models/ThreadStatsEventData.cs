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
    /// Represent statistics of a Thread.
    /// </summary>
    public class ThreadStatsEventData
    {
        /// <summary>
        /// ID of the Thread.
        /// </summary>
        public string ThreadId { get; set; } = null!;

        /// <summary>
        /// Last message timestamp.
        /// </summary>
        public long LastMsgDate { get; set; }

        /// <summary>
        /// Total number of messages in the Thread.
        /// </summary>
        public long MessagesCount { get; set; }
    }
}
