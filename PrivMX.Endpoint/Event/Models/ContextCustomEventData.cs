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

namespace PrivMX.Endpoint.Event.Models
{
    /// <summary>
    /// Holds information of <see cref="ContextCustomEvent"/> event data.
    /// </summary>
    public class ContextCustomEventData
    {
        /// <summary>
        /// Context ID.
        /// </summary>
        public string ContextId { get; set; }

        /// <summary>
        /// User ID (event's sender).
        /// </summary>
        public string UserId { get; set; }

        /// <summary>
        /// Event's actual payload.
        /// </summary>
        public byte[] Payload { get; set; }
    }
}
