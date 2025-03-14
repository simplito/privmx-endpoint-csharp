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

namespace PrivMX.Endpoint.Core.Models
{
    /// <summary>
    /// Represents an event.
    /// </summary>
    public class Event
    {
        /// <summary>
        /// Type of event.
        /// </summary>
        public string Type { get; set; }

        /// <summary>
        /// Additional identifier of the subscribed source module and resource.
        /// </summary>
        public string Channel { get; set; }

        /// <summary>
        /// ID of the connection emitting the event.
        /// </summary>
        public long ConnectionId { get; set; }
    }
}
