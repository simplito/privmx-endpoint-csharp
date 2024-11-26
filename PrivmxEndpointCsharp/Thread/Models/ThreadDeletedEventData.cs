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
    /// Represents payload of the ThreadDeletedEvent.
    /// </summary>
    public class ThreadDeletedEventData
    {
        /// <summary>
        /// ID of the deleted Thread.
        /// </summary>
        public string ThreadId { get; set; }
    }
}
