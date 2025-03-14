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

using PrivMX.Endpoint.Core.Models;

namespace PrivMX.Endpoint.Store.Models
{
    /// <summary>
    /// Represents the event of type "storeFileCreated".
    /// This event is emitting when file events are subscribed and a new file is created.
    /// </summary>
    public class StoreFileCreatedEvent : Event
    {
        /// <summary>
        /// Created file.
        /// </summary>
        public File Data { get; set; }
    }
}
